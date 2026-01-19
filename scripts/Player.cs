using System.Runtime.CompilerServices;
using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float _speed = 100f;
    [Export] public float _friction = 0.2f;
    [Export] public float _acceleration = 0.2f;
	[Export] public float _health = 100.0f;

	private bool isAttacking = false;

	private AnimationPlayer _animationPlayer;
	private Sprite2D _sprite;

    public override void _Ready()
    {
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("Animation");
    }

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
		if (_sprite == null || _animationPlayer == null)
			return;

		Vector2 input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (isAttacking)
			return;

		if (input.Y < 0)
		{
			_animationPlayer.Play("Running_up");
		}
		else if (input.Y > 0)
		{
			_animationPlayer.Play("Running_down");
		}
		else if (input.X != 0)
		{
			_sprite.FlipH = input.X < 0;
			_animationPlayer.Play("Running");
		}
		else
		{
			_animationPlayer.Play("Idle");
		}

		if (Input.IsActionJustPressed("attack"))
		{
			_animationPlayer.Play("Attack_down");
		}
		
	}

	async void Attack()
	{
		if(isAttacking)
			return;			

		isAttacking = true;
		_animationPlayer.Play("Attack_down");
		
		await ToSignal(_animationPlayer, "animation_finished");

		isAttacking = false;
	}

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
		if (Input.IsActionJustPressed("attack"))
		{
			Attack();
		}
		UpdateAnimation();
        MoveAndSlide();
    }
}
