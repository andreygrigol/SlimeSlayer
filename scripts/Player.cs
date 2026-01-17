using Godot;

public partial class Player : CharacterBody2D
{
    [ExportCategory("Variables")]
    [Export] public float _speed = 100.0f;
    [Export] public float _friction = 0.2f;
    [Export] public float _acceleration = 0.2f;
	[Export] public float _health = 100.0f;

	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite;

    private void GetInput()
    {
        Vector2 inputDirection = Input.GetVector(
            "ui_left",
            "ui_right",
            "ui_up",
            "ui_down"
        );

        if (inputDirection != Vector2.Zero)
        {
            Vector2 dir = inputDirection.Normalized();

            Velocity = new Vector2(
                Mathf.Lerp(Velocity.X, dir.X * _speed, _acceleration),
                Mathf.Lerp(Velocity.Y, dir.Y * _speed, _acceleration)
            );

            return;
        }

        Velocity = new Vector2(
            Mathf.Lerp(Velocity.X, 0, _friction),
            Mathf.Lerp(Velocity.Y, 0, _friction)
        );
    }

	private void UpdateAnimation()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("Animation");

		if (_sprite == null || _animationPlayer == null)
			return;

		Vector2 input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (input.Y < 0)
		{
			PlayAnimation("Running_up");
		}
		else if (input.Y > 0)
		{
			PlayAnimation("Running_down");
		}
		else if (input.X != 0)
		{
			_sprite.FlipH = input.X < 0;
			PlayAnimation("Running");
		}
		else
		{
			PlayAnimation("Idle");
		}
	}

	private void PlayAnimation(string name)
	{
		if (_animationPlayer.CurrentAnimation != name)
			_animationPlayer.Play(name);
	}


    public override void _PhysicsProcess(double delta)
    {
        GetInput();
		UpdateAnimation();
        MoveAndSlide();
    }
}
