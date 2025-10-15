using System;
using System.IO;
using Godot;

public partial class CharachterPatrol : Node3D
{
    private PathFollow3D _pathFollow;
    private Timer _waypointTimer;

    [Export]
    private float _totalLoopTime;
    private float _currentPathTime;
    private Vector3[] _waypointPositions;
    private int _currentWaypointIndex;
    private bool _isMooving = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _waypointTimer = GetNode<Timer>("WaypointTimer");
        _waypointTimer.Timeout += _OnWaypointTimerDone;

        _pathFollow = GetParent<PathFollow3D>();
        Curve3D pathCurve = _pathFollow.GetParent<Path3D>().Curve;

        _waypointPositions = new Vector3[pathCurve.PointCount];
        for (int i = 0; i < (pathCurve.PointCount - 1); i++)
        {
            _waypointPositions[i] = pathCurve.GetPointPosition(i);
            GD.Print("i : ", i, "   value:", pathCurve.GetPointPosition(i));
        }
        _currentWaypointIndex = 0;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_isMooving)
        {
            _currentPathTime += (float)delta;
            _pathFollow.ProgressRatio = _currentPathTime / _totalLoopTime;

            float d = (
                _waypointPositions[(_currentWaypointIndex + 1) % _waypointPositions.Length]
                - _pathFollow.Position
            ).Length();
            if (d < 0.1f)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypointPositions.Length;
                StartWaiting();
            }
        }
    }

    private void StartWaiting()
    {
        _isMooving = false;
        _waypointTimer.Start();
    }

    private void _OnWaypointTimerDone()
    {
        ;
        _isMooving = true;
    }
}
