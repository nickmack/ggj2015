using UnityEngine;

/// <summary>
/// Helper for periodic events
/// </summary>
public class Every
{
	/// <summary>
	/// Constructs an Every object
	/// </summary>
	/// <param name="interval">Interval in which event is to occur</param>
	public Every(float interval)
	{
		_interval = interval;
		_tickCount = 0;
		_lastMoment = Time.time;
	}
	
	/// <summary>
	/// Returns True if the event should happen, false otherwise.
	/// <remarks>Setting this property to True forces the IsTriggered to fire on next call</remarks>
	/// </summary>
	public bool IsTriggered
	{
		get
		{
			_tickCount = 0;
			
			float currentTime = Time.time;
			
			if (currentTime < _lastMoment + _interval)
			{
				return false;
			}
			else if (currentTime == _lastMoment + _interval)
			{
				return true;
			}
			else
			{
				_tickCount = (int)((currentTime - _lastMoment) / _interval) - 1;
				_lastMoment += _interval;
				return true;
			}
		}
		set
		{
		/*
         * Setting this property to True will cause the IsTriggered to return True on the next call.
         *
         * Why is this useful? For example, if you have something that happens every 60 seconds, you would
         * create this object with
         *
         * Every someEvent = new Every(60);
         *
         * However, the event will fire After first 60 seconds pass. If you want it to fire upon creation
         * you can set it to True and it will fire on next IsTriggered query. This is only an example,
         * setting this property basically forces the event to fire, even if time has not yet passed.
         * However, new period is calculated from the time you set this property to true, not from the
         * initialy set time. But that's obvious.
         */
			if (value)
				_lastMoment -= _interval;
		}
	}
	
	/// <summary>
	/// Returns true if events accumulated after the last call to IsTriggered
	/// </summary>
	private bool IsLate
	{
		get
		{
			return _tickCount > 0;
		}
	}
	
	/// <summary>
	/// Returns the number of accumulated events after the last call to IsTriggered.
	/// </summary>
	private int LateCount
	{
		get
		{
			return _tickCount;
		}
	}
	
	private int _tickCount;
	private float _lastMoment;
	private float _interval;
}
