using Godot;
using System;

namespace TowerDefense.Tutorial02_Base
{
	
	public partial class TowerToPlaceManager : Node2D
	{
		private static Color _COLOR_VALID = new Color("#4ef544");
		private static Color _COLOR_INVALID = new Color("#f55544");
		private const string _DEFAULT_TOWER_MATERIAL = "res://art/materials/TowerTransparent.tres";
		private const string _BASE_NODE_PATH = "Base";
		private const string _CANON_NODE_PATH = "Canon";
		
		private ShaderMaterial _spritesMaterial;
		
		// tower parameters
		public float radius = 400f; // in pixels

		public int cost = 20;

		public override void _Ready()
		{
			_InitializeSpriteMaterial();
			string radiusDisplayPath = "RadiusDisplay";
			ShaderMaterial m = (ShaderMaterial) GetNode<CanvasItem>(radiusDisplayPath).Material;
			m.SetShaderParameter("radius", radius / GetNode<MeshInstance2D>(radiusDisplayPath).Scale.X);
		}
		
		public void SetValid(bool isValid)
		{
			if (_spritesMaterial == null)
			{
				_InitializeSpriteMaterial();
				if (_spritesMaterial == null)
				{
					return;
				}
			}
			
			Color c = isValid ? _COLOR_VALID : _COLOR_INVALID;
			_spritesMaterial.SetShaderParameter("tint", c);
		}

		private void _InitializeSpriteMaterial()
		{
			CanvasItem baseNode = GetNodeOrNull<CanvasItem>(_BASE_NODE_PATH);
			if (baseNode == null)
			{
				GD.PushError($"Tower placement node is missing '{_BASE_NODE_PATH}' child.");
				return;
			}
			
			_spritesMaterial = baseNode.Material as ShaderMaterial;

			if (_spritesMaterial == null)
			{
				ShaderMaterial template = GD.Load<ShaderMaterial>(_DEFAULT_TOWER_MATERIAL);
				if (template != null)
				{
					_spritesMaterial = (ShaderMaterial) template.Duplicate();
					_spritesMaterial.ResourceLocalToScene = true;
				}
				else
				{
					GD.PushError($"Failed to load tower placement material at '{_DEFAULT_TOWER_MATERIAL}'.");
					_spritesMaterial = new ShaderMaterial();
				}

				baseNode.Material = _spritesMaterial;
			}

			CanvasItem canonNode = GetNodeOrNull<CanvasItem>(_CANON_NODE_PATH);
			if (canonNode != null)
			{
				canonNode.Material = _spritesMaterial;
			}
		}
	}
	
}
