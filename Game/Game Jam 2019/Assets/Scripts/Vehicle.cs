using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Vehicle : MonoBehaviorExtended
{
    private const string VECTOR_MANAGEMENT_OBJECT_NAME = "VectorManager";

    private GameObject vectorDirectionalObject;
    private Vector2 forwardVector, rightVector;
    private Vector2 previousVelocity = Vector2.zero;
    private List<Transform> pathNodes;

    protected virtual float MaxSpeed
    {
        get { return 100; }
    }

    protected virtual float MaxForce
    {
        get { return 2.5f; }
    }

    private List<GameObject> allOtherObjectsOnScreen;
    protected virtual List<GameObject> AllOtherObjectsOnScreen
    {
        get
        {
            if (allOtherObjectsOnScreen == null)
            {
                List<GameObject> allObjects = GlobalGameManager.ObjectsOnScreenWithColliders;
                
                // Remove all children of this object from the list to get all others
                foreach (Transform child in transform)
                {
                    allObjects.Remove(child.gameObject);
                }

                // Remove this object as well (the parent)
                allObjects.Remove(gameObject);

                allOtherObjectsOnScreen = allObjects;
            }
            return allOtherObjectsOnScreen;
        }
    }


    protected Vector2 Acceleration
    {
        get; set;
    }

    protected Vector2 Velocity
    {
        get; set;
    }

    protected Vector2 DesiredVelocity
    {
        get; set;
    }

    /// <summary>
    /// Get the embedded object handling vector directions
    /// </summary>
    protected GameObject VectorDirectionalObject
    {
        get
        {
            // Do we not have one for this object?
            if (vectorDirectionalObject == null)
            {
                // Okay we don't, let's see if we can find if this object has a VectorManager empty gameobject already
                if (transform.Find(VECTOR_MANAGEMENT_OBJECT_NAME) == null)
                {
                    // Okay! Well let's just create a new one as child of this object
                    GameObject vectorObject = new GameObject(VECTOR_MANAGEMENT_OBJECT_NAME);
                    vectorObject.transform.parent = transform;
                }
                // Okay no matter what we should have a vector managing object
                vectorDirectionalObject = transform.Find(VECTOR_MANAGEMENT_OBJECT_NAME).gameObject;
            }
            // Return that vector object now!
            return vectorDirectionalObject;
        }
    }
    protected Vector2 ForwardVectorInChild
    {
        get
        {
            return VectorDirectionalObject.transform.up;
        }
    }

    protected Vector2 RightVectorInChild
    {
        get
        {
            return VectorDirectionalObject.transform.right;
        }
    }

    protected virtual void Start()
    {
        Acceleration = Vector2.zero;
        Velocity = transform.forward;
    }

    protected virtual void Update()
    {
        CalcSteeringForces();
        //Update the velocity on the vehicle
        Velocity += Acceleration * Time.deltaTime;  //Smooth movement independent of framerate
        Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);
        //ClampMagnitude(Vector to be limited, value to be limited by)

        //Orient our transform to face where we're going
        if (Velocity != Vector2.zero)
        {
            //If he's not facing the way we want, point him towards that way
            VectorDirectionalObject.transform.up = Velocity.normalized;
        }

        //Call character controller and ask it to move, i.e. ADD VELOCITY TO POSITION
        MoveObjectToNewPosition(Velocity * Time.deltaTime);
        
        //Reset acceleration for next cycle
        Acceleration = Vector3.zero;
    }

    /// <summary>
    /// Required method for all inheriting members, handles movement behavior
    /// </summary>
    abstract protected void CalcSteeringForces();


    protected void ApplyForce(Vector2 steeringForce)
    {
        //ADD THE FORCE DIVIDED BY THE MASS TO THE ACCELERATION//
        Acceleration += steeringForce;
    }

    /// <summary>
    /// Vehicle seeks and doesn't stop chasing, great for enemies
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    protected Vector2 Seek(Vector2 targetPosition)
    {
        Vector2 force = Vector2.zero;

        DesiredVelocity = targetPosition - RigBody.position;
        DesiredVelocity = DesiredVelocity.normalized;
        DesiredVelocity *= MaxSpeed;
        force = DesiredVelocity - Velocity;
        

        //Desired Velocity = Target - Position
        //Normalize desired velocity
        //Multiply desired velocity by maximum speed
        //Steering force = Desired velocity - Vehicle's Velocity

        return force;
    }

    /// <summary>
    /// Vehicle seeks, but slows down as it reaches target, great for minions
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    protected Vector2 Arrive(Vector2 targetPosition)
    {
        Vector2 force = Vector2.zero;
        DesiredVelocity = targetPosition - RigBody.position;
        DesiredVelocity = DesiredVelocity.normalized;
        DesiredVelocity *= MaxSpeed;
        float decelerationSpeed = Mathf.Pow(Vector2.Distance(targetPosition, gameObject.transform.position), 4) / MaxSpeed;
        DesiredVelocity *= decelerationSpeed;
        force = DesiredVelocity - Velocity;
        return force;
    }

    /// <summary>
    /// Vehicle flees from target (Works opposite to seeking)
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    protected Vector2 Flee(Vector2 targetPosition)
    {
        Vector2 force = Vector2.zero;

        DesiredVelocity = targetPosition - RigBody.position;
        DesiredVelocity = DesiredVelocity.normalized;
        DesiredVelocity *= -MaxSpeed;
        force = DesiredVelocity - Velocity;
        //Desired Velocity = Target - Position
        //Normalize desired velocity
        //Multiply desired velocity by the NEGATIVE maximum speed
        //Steering force = Desired velocity - Vehicle's Velocity

        return force;
    }

    /// <summary>
    /// Vehicle avoids obstacles
    /// </summary>
    /// <param name="safeDistance">Distance at which enemy will begin avoiding obstacle</param>
    /// <returns></returns>
    protected Vector2 AvoidObstacle(float safeDistance)
    {
        Vector2 force = Vector2.zero;
        List<GameObject> obstaclesToAvoid = AllOtherObjectsOnScreen;
        // We don't want to avoid player, defeats the purpose
        obstaclesToAvoid.Remove(GameObject.FindGameObjectWithTag(GlobalGameManager.PLAYER_TAG));
        
        // Go through all obstacles we want to avoid
        foreach (GameObject obstacle in obstaclesToAvoid)
        {
            Vector2 avoidVelocity = Vector2.zero;
            Vector2 directionToCollision = (obstacle.transform.position - gameObject.transform.position).normalized;

            // Raycast a safe distance away to see if it's going to hit something
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToCollision, safeDistance);

            // Is the vehicle about to hit something???
            if (hit)
            {
                // Is this thing that got hit the obstacle??
                if (hit.transform != transform && hit.transform.gameObject.Equals(obstacle))
                {
                    // Just some vector math
                    Vector2 playerPos = transform.position;
                    Vector2 directionOfHit = hit.point - playerPos;
                    float dotProdToRight = Vector2.Dot(directionOfHit, RightVectorInChild);
                    float dotProdToForward = Vector2.Dot(directionOfHit, RightVectorInChild);
                    float distance = Vector2.Distance(hit.point, playerPos);
                    
                    // Move to the right!
                    if (dotProdToRight > 0)
                    {
                        avoidVelocity = RightVectorInChild * (-MaxSpeed * safeDistance / (distance / 2));
                    }

                    //Move to the left!
                    else
                    {
                        avoidVelocity = RightVectorInChild * (MaxSpeed * safeDistance / (distance / 2));
                    }

                    // God forbid anything bad happens with object avoidance detection, we can't move something if its' vector is malformed, so just make sure we don't have that happen
                    if(avoidVelocity.x == float.NaN || float.IsInfinity(avoidVelocity.x) || avoidVelocity.y == float.NaN || float.IsInfinity(avoidVelocity.y))
                    {
                        avoidVelocity = Vector2.zero;
                    }
                }
            }
            // Vehicle isn't going to hit anything, we can ignore any obstacle avoidance, cool!
            else
            {
                continue;
            }
            
            // Apply our avoidance!
            force += avoidVelocity;
            

            // OLD OBSTACLE AVOIDANCE CODE
            /*Vector2 toCenter = obstacle.transform.position - gameObject.transform.position;
            float dotProdToCenterRight = Vector2.Dot(toCenter, RightVectorInChild);
            float dotProdToCenterFwd = Vector2.Dot(toCenter, ForwardVectorInChild);
            float distance = Vector2.Distance(obstacle.transform.position, gameObject.transform.position);
            


            Vector3 colliderExtents = obstacle.GetComponent<BoxCollider2D>().bounds.extents;
            float obstRadius = Vector3.Magnitude(colliderExtents);

            if ((distance > safeDistance + obstRadius) ||
                (dotProdToCenterFwd < 0) ||
                (Mathf.Abs(dotProdToCenterRight) > obstRadius))
            {
                continue;
            }


            Vector2 avoidVelocity = Vector2.zero;
            
            
            if (dotProdToCenterRight > 0)
            {
                avoidVelocity = RightVectorInChild * (-MaxSpeed * safeDistance / distance);
            }
            else
            {
                avoidVelocity = RightVectorInChild * (MaxSpeed * safeDistance / distance);
            }
            force += avoidVelocity;*/
        }

        return force;

    }

    /*public Vector2 FlockingForGroupOfObjects(List<GameObject> flockGroup, float separationDistance)
    {
        return Vector2.zero;
    }

    /// <summary>
    /// FLOCKING MECHANIC - How far apart should a flock be
    /// </summary>
    /// <param name="flockers"></param>
    /// <param name="separationDistance"></param>
    /// <returns></returns>
    public Vector2 FlockSeparation(List<GameObject> flockGroup, float separationDistance)
    {
        Vector2 force = Vector2.zero;

        foreach (GameObject g in flockGroup)
        {
            //Get distance between me and each of the neighbors
            DesiredVelocity = gameObject.transform.position - g.gameObject.transform.position;
            float dist = DesiredVelocity.magnitude;

            //If neighbor is too close
            if (dist > 0 && dist < separationDistance)
            {
                DesiredVelocity *= separationDistance / dist;
                force += DesiredVelocity;
            }


        }
        //Move me in correct direction
        force = force.normalized * MaxSpeed;
        force -= Velocity;
        return force;

    }

    /// <summary>
    /// FLOCKING MECHANIC - Alignment of the flock
    /// </summary>
    /// <param name="alignVector"></param>
    /// <returns></returns>
    public Vector2 FlockAlignment(Vector3 alignVector)
    {
        Vector2 force = Vector2.zero;

        alignVector = alignVector.normalized;
        DesiredVelocity = alignVector * MaxSpeed;

        return DesiredVelocity - Velocity;

        //Create a Vector3 that will reference the average vector which is the overall direction
        //Normalize this vector
        //Create a desiredVelocity = averageDirection * maxSpeed
        //Steer = desiredVelocity - vehicle's velocity


    }

    /// <summary>
    /// Indicates the center of the Flock
    /// </summary>
    /// <param name="cohesionVector"></param>
    /// <returns></returns>
    public Vector2 FlockCohesion(Vector2 cohesionVector)
    {
        //GO TO THE CENTER
        return Seek(cohesionVector);

    }*/

    /// <summary>
    /// For patrolling vehicles make it so they will prefer to go back to patrol rather than chase player after X radius size
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="center"></param>
    /// <returns></returns>
    public Vector2 StayNearPatrolCenter(GameObject pathNodeParentObject, float radius)
    {
        Vector2 center = pathNodeParentObject.transform.position;
        if (Vector2.Distance(gameObject.transform.position, center) > radius)
        {
            return Seek(center);
        }
        else
        {
            return Vector2.zero;
        }
    }


    /// <summary>
    /// Make vehicles follow in a set of child path nodes from a parent path node object
    /// </summary>
    /// <param name="pathNodes"></param>
    /// <returns></returns>
    protected Vector2 PathFollow(GameObject pathNodeParentObject)
    {
        if(pathNodes == null)
        {
            pathNodes = new List<Transform>();
            foreach (Transform child in pathNodeParentObject.transform)
                pathNodes.Add(child.transform);
        }

        
        int closestNodeIndex = DetectClosestNodeIndex(pathNodes);

        Transform closestNode = pathNodes[closestNodeIndex];
        Transform nextNode;
        Transform followingNode;


        // Choose next node
        if (closestNodeIndex + 1 > pathNodes.Count - 1)
        {
            nextNode = pathNodes[0];
        }
        else
        {
            nextNode = pathNodes[closestNodeIndex + 1];
        }

        // Pre-choose the node that will follow the next node
        if (closestNodeIndex + 2 > pathNodes.Count - 1)
        {
            followingNode = pathNodes[1];
        }
        else
        {
            followingNode = pathNodes[closestNodeIndex + 2];
        }
        
        

        Vector2 segment = nextNode.position - closestNode.position;
        
        // Estimate based on direction what the players next point will be
        Vector3 futurePoint = ForwardVectorInChild * 1.5f;
        Vector2 closestNodeToFuturePoint = futurePoint - pathNodes[closestNodeIndex].transform.position;
        float projectionOfFuturePos = Vector2.Dot(closestNodeToFuturePoint, segment);

        
        Vector2 pathFollowVelocity = Vector2.zero;
        if (projectionOfFuturePos < segment.magnitude)
        {
            pathFollowVelocity = nextNode.position - transform.position;
        }
        else
        {
            pathFollowVelocity = followingNode.position - transform.position;
        }
        
        pathFollowVelocity = pathFollowVelocity.normalized;
        
        
        return pathFollowVelocity * MaxSpeed;
        
    }

    /// <summary>
    /// Find which node is the closest in a series of nodes
    /// </summary>
    /// <param name="pathNodes"></param>
    /// <returns></returns>
    private int DetectClosestNodeIndex(List<Transform> pathNodes)
    {
        List<float> nodeDistances = new List<float>();
        float winningMag = 0.0f;
        int shortestDistIndex = 0;
        for (int x = 0; x < pathNodes.Count; x++)
        {
            Vector2 distToPlayer = pathNodes[x].transform.position - transform.position;
            nodeDistances.Add(distToPlayer.magnitude);
            if (nodeDistances.Count > 1)
            {
                if (nodeDistances[x] < nodeDistances[x - 1] && nodeDistances[x] < winningMag)
                {
                    winningMag = nodeDistances[x];
                    shortestDistIndex = x;
                }
            }
            else
            {
                winningMag = nodeDistances[x];
            }

        }

        return shortestDistIndex;
    }
}
