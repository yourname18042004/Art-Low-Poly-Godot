using Godot;
using System;

public partial class player : CharacterBody3D
{
    public float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    public Vector3 _mouse_rotation = new Vector3(0, 0, 0);
    public double _rotation_input;
    public double _tilt_input;
    public Vector3 _player_rotation;
    public Vector3 _camera_rotation;

    public double MOUSE_SENSITIVITY = 0.5;
    public double TILT_LOWER_LIMIT = Mathf.DegToRad(-90.0f);
    public double TILT_UPPER_LIMIT = Mathf.DegToRad(90.0f);
    public Node3D CAMERA_CONTROLLER = null;

    public Vector3 velocity;
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        CAMERA_CONTROLLER = this.GetNode<Godot.Node3D>("CameraController");

        velocity = Velocity;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("exit"))
        {
            GetTree().Quit();
        }
    }


    public void _UpdateCamera(double delta)
    {
        _mouse_rotation.X += (float)(_tilt_input * delta * MOUSE_SENSITIVITY);
        _mouse_rotation.X = (float)Mathf.Clamp(_mouse_rotation.X, TILT_LOWER_LIMIT, TILT_UPPER_LIMIT);
        _mouse_rotation.Y += (float)(_rotation_input * delta * MOUSE_SENSITIVITY);

        _player_rotation = new Vector3(0, _mouse_rotation.Y, 0);
        _camera_rotation = new Vector3(_mouse_rotation.X, 0, 0);

        CAMERA_CONTROLLER.Rotation = _camera_rotation;
        this.Rotation = _player_rotation;

        _rotation_input = 0;
        _tilt_input = 0;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouseMotion inputEventMouseMotion)
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                _rotation_input = -inputEventMouseMotion.Relative.X;
                _tilt_input = -inputEventMouseMotion.Relative.Y;
            }
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        _UpdateCamera(delta);

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;
        update_gravity(delta);
        update_input(5, 0.1f, 0.25f);
        update_velocity();
    }

    public void update_gravity(double delta) 
    {
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;
    }

    public void update_input(float speed, float acceleration, float deceleration)
    { 
        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = Mathf.Lerp(velocity.X, direction.X * speed, acceleration);
            velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * speed, acceleration);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, deceleration);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, deceleration);
        }
    }

    public void update_velocity()
    {
        Velocity = velocity;
        MoveAndSlide();
    }
}
