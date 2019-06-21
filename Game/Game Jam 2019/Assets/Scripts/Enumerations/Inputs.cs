using UnityEngine;

public static class Inputs
{
    private static readonly string
        HorizontalInputAxis = "Horizontal",
        VerticalInputAxis = "Vertical",
        ShootingHorizontalInputAxis = "ShootingX",
        ShootingVerticalInputAxis = "ShootingY",
        Fire1 = "Fire1",
        Fire2 = "Fire2",
        Fire3 = "Fire3",
        Jump = "Jump",
        MouseX = "Mouse X",
        MouseY = "Mouse Y",
        MouseScroll = "Mouse ScrollWheel",
        Submit = "Submit",
        Cancel = "Cancel";


    public static float HorizontalInput
    {
        get
        {
            return Input.GetAxis(HorizontalInputAxis);
        }
    }

    public static float VerticalInput
    {
        get
        {
            return Input.GetAxis(VerticalInputAxis);
        }
    }

    public static float ShootingHorizontalInput
    {
        get
        {
            return Input.GetAxis(ShootingHorizontalInputAxis);
        }
    }

    public static float ShootingVerticalInput
    {
        get
        {
            return Input.GetAxis(ShootingVerticalInputAxis);
        }
    }

    public static float MouseXInput
    {
        get
        {
            return Input.GetAxis(MouseX);
        }
    }

    public static float MouseYInput
    {
        get
        {
            return Input.GetAxis(MouseY);
        }
    }

    public static float MouseScrollInput
    {
        get
        {
            return Input.GetAxis(MouseScroll);
        }
    }

    public static bool IsButton1Pressed
    {
        get
        {
            return Input.GetAxis(Fire1) == 1;
        }
    }

    public static bool IsButton2Pressed
    {
        get
        {
            return Input.GetAxis(Fire2) == 1;
        }
    }

    public static bool IsButton3Pressed
    {
        get
        {
            return Input.GetAxis(Fire3) == 1;
        }
    }

    public static bool IsJumpPressed
    {
        get
        {
            return Input.GetAxis(Jump) == 1;
        }
    }

    public static bool IsConfirmPressed
    {
        get
        {
            return Input.GetAxis(Submit) == 1;
        }
    }

    public static bool IsCancelPressed
    {
        get
        {
            return Input.GetAxis(Cancel) == 1;
        }
    }

}
