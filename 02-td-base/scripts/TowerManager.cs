using Godot;
using System;
using System.Collections.Generic;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class TowerManager : Area2D
	{
		[Export] private PackedScene _cannonBallAsset;
		private Node2D _currentTarget;
		private List<Node2D> _targetsInRange;
		private Node2D _cannonSprite;
		private CollisionShape2D _fovAreaShape;

		private LevelManager _levelManager;		
		
		public override void _Ready()
		{
			MouseEntered += _OnTowerMouseEntered;
			MouseExited += _OnTowerMouseExited;

			_targetsInRange = new List<Node2D>();
			_currentTarget = null;

			_cannonSprite = GetNode<Node2D>("Cannon");
		}

        public override void _Process(double delta)
        {
            if(_currentTarget != null)
            {
                _cannonSprite.LookAt(_currentTarget.Position);
            }
        }

		public void Initialize(LevelManager levelManager, float radius)
		{
			_levelManager = levelManager;
			
			_fovAreaShape = GetNode<CollisionShape2D>("FOVArea2D/CollisionShape2D");
			((CircleShape2D)_fovAreaShape.Shape).Radius = radius;

			Area2D FOV = (Area2D) _fovAreaShape.GetParent();
			FOV.AreaEntered += _OnFOVAreaEntered;
			FOV.AreaExited += _OnFOVAreaExited;

		}

		private void _OnTowerMouseEntered()
        {
			_levelManager.SetCanPlaceTower(false);
        }
		private void _OnTowerMouseExited()
        {
            _levelManager.SetCanPlaceTower(true);
        }
		private void _OnFOVAreaEntered(Area2D area)
        {
			GD.Print("ENTERDE");
			Node2D ship = area.GetParent<Node2D>();
			_targetsInRange.Add(ship);
            if (_currentTarget == null)
            {
                _currentTarget = _targetsInRange[0];
            }
        }

		private void _OnFOVAreaExited(Area2D area)
        {
			Node2D ship = area.GetParent<Node2D>();
			_targetsInRange.Remove(ship);
			if(_currentTarget == ship)
            {
                _currentTarget = _targetsInRange[0] ?? null;
            }

        }
	}
	
}
