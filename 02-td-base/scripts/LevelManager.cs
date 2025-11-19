using Godot;
using System;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class LevelManager : Node2D
	{
		[Export] private PackedScene _shipAsset;
		[Export] private PackedScene _towerAsset;
		private TowerToPlaceManager _towerToPlace;
		private bool _towerHasValidPlacement;
		private bool _canPlaceTower;
		private Path2D _path;
		private bool _isBuilding;
		private TileMapLayer _groundTilemap;
		private float _cellRound;
		private Vector2 _cellOffset;
		public override void _Ready()
		{
			_path = GetNode<Path2D>("Path2D");
			_towerToPlace = GetNode<TowerToPlaceManager>("/root/Base/Tower-ToPlace");
			_groundTilemap =GetNode<TileMapLayer>("Tilemaps/ground");
			_cellRound = _groundTilemap.TileSet.TileSize.X;
			_cellOffset = 0.5f * new Vector2(_cellRound, _cellRound);
			SetIsBuilding(false);

			Node towerButtonsParrent = GetNode<Node>("/root/Base/CanvasLayer/UI/Towers");
			for (int i = 0; i < towerButtonsParrent.GetChildCount(); i ++)
            {
				if (towerButtonsParrent.GetChild(i) is BaseButton button)
				{
					button.Pressed += _OnTowerButtonPressed;
					button.MouseEntered += _OnTowerButtonMouseEntered;
					button.MouseExited += _OnTowerButtonMouseExited;
				}				
            }
		}
		
		private void _OnEnemySpawn()
		{
			Node ship = _shipAsset.Instantiate();
			_path.AddChild(ship);
		}
		
		public override void _Input(InputEvent @event)
        {
            if(@event is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == MouseButton.Left && !eventMouseButton.Pressed)
            {
                if(_towerHasValidPlacement && _isBuilding && _canPlaceTower)
                {
                    if (GameManager.instance.BuyTower(_towerToPlace))
                    {
                         _PlaceTower(_RoundPositionToTileMap(GetGlobalMousePosition()));
                    }
                }
            }else if (@event is InputEventMouseMotion eventMouseMove)
            {
                Vector2 mousePos = GetGlobalMousePosition();
				_towerToPlace.Position = _RoundPositionToTileMap(mousePos);

				Vector2I cellPos = _groundTilemap.LocalToMap(_towerToPlace.Position);
				_towerHasValidPlacement = _groundTilemap.GetCellSourceId(cellPos) != -1;
				_towerToPlace.SetValid(_towerHasValidPlacement);
            }
        }

		private Vector2 _RoundPositionToTileMap(Vector2 p) => (p/_cellRound).Floor() * _cellRound + _cellOffset;

		private void _PlaceTower(Vector2 pos)
		{
			Node2D tower = (Node2D) _towerAsset.Instantiate();
			tower.Position = pos;
			AddChild(tower);
			((TowerManager)tower).Initialize(this, _towerToPlace.radius);
			SetIsBuilding(false);
        }

		private void SetIsBuilding(bool isBuilding)
        {
            _isBuilding = isBuilding;
            if (_isBuilding && _canPlaceTower)
            {
                _towerToPlace.Show();
            }
            else
            {
                _towerToPlace.Hide();
            }
        }

		public void SetCanPlaceTower(bool canPlaceTower)
        {
			_canPlaceTower = canPlaceTower;
			if(_canPlaceTower && _isBuilding)
            {
                _towerToPlace.Show();
            }
            else
            {
                _towerToPlace.Hide();
            }
        }
		
		private void _OnTowerButtonPressed()
        {
            SetIsBuilding(true);
        }
		private void _OnTowerButtonMouseEntered()
        {
            SetCanPlaceTower(false);
        }
		private void _OnTowerButtonMouseExited()
        {
            SetCanPlaceTower(true);
        }
	}
}
