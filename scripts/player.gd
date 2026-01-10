extends CharacterBody2D

@export_category("Variables")

@export var _speed: float = 100.0
@export var _friction: float = 0.2
@export var _acceleration: float = 0.2


func get_input():
	var _input_direction = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")

	if _input_direction != Vector2.ZERO:
		velocity.x = lerp(velocity.x, _input_direction.normalized().x * _speed, _acceleration)
		velocity.y = lerp(velocity.y, _input_direction.normalized().y * _speed, _acceleration)
		return

	velocity.x = lerp(velocity.x, _input_direction.normalized().x * _speed, _friction)
	velocity.y = lerp(velocity.y, _input_direction.normalized().y * _speed, _friction)

func _physics_process(_delta):
	get_input()
	move_and_slide()