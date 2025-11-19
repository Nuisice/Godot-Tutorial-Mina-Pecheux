using Godot;
using System;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class ShipManager : PathFollow2D
	{
		private PathFollow2D _pathFollow;
		
		// ship parameters
		private float _speed = 1f;
		public int HP = 1;
		public int reward = 1;
		public int damage = 1;
		
		public override void _Ready()
		{
			_pathFollow = GetNode<PathFollow2D>(GetPath());
		}

		public override void _Process(double delta)
		{
			_pathFollow.ProgressRatio += (float)delta * _speed * 0.03f;
			if (_pathFollow.ProgressRatio >= 1)
				_Pass();
		}
		
		private void _Pass()
		{
			GameManager.instance.OnShipPassed(this);
			QueueFree();
		}
		
		private void _OnArea2DBodyEntered(Node2D body)
		{
			CannonBallManager cannonBall = (CannonBallManager) body;
			
			cannonBall.QueueFree();
		}

		private void OnHit(CannonBallManager cannonBall)
        {
            HP -= cannonBall.damage;
			if(HP <= 0){
                _Die();
            }
        }

		private void _Die()
        {
            GameManager.instance.OnShipDie(this);
			QueueFree();
        }
	}
	
}
