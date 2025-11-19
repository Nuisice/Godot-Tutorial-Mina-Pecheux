using Godot;
using System;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class GameManager : Node
	{
		public static GameManager instance;
		private Label _coinsLabel;
		private Label _livesLabel;
		private int _coins = 40;
		private int _lives = 3;

        public override void _Ready()
        {
			instance = this;

            _coinsLabel = GetNode<Label>("CanvasLayer/UI/Stats/Coins/Label");
            _livesLabel = GetNode<Label>("CanvasLayer/UI/Stats/Lives/Label");
			_UpdateUI();
        }

		private void _UpdateUI()
        {
            _coinsLabel.Text = _coins.ToString();
			_livesLabel.Text = _lives.ToString();
        }

		public void OnShipPassed(ShipManager ship)
        {
            _lives -= ship.damage;
			_UpdateUI();
        }

		public void OnShipDie(ShipManager ship)
        {
            _coins += ship.reward;
			_UpdateUI();
        }

		public bool BuyTower(TowerToPlaceManager towerToPlace)
        {
			if(_coins < towerToPlace.cost) return false;
            _coins -= towerToPlace.cost;
			_UpdateUI();
			return true;
        }

	}
	
}
