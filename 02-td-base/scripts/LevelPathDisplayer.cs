using Godot;
using System;

namespace TowerDefense.Tutorial02_Base
{
	public partial class LevelPathDisplayer : Node2D
	{	
		private Curve2D _pathCurve;
		private Color _pathColor;
		private float _pathWidth = 15f;
        
		public override void _Ready()
        {
            Path2D path = GetNode<Path2D>("/root/Base/Level/Path2D");
			_pathCurve = path.Curve;
			_pathColor = new Color(1,0,0,0.5f);
        }

        public override void _Draw()
        {
         DrawPolyline(_pathCurve.GetBakedPoints(), _pathColor, _pathWidth, true);   
        }
	}
	
}
