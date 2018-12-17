using UnityEngine;
using VolumetricLines;

namespace NFLCryptoFootball.UI
{
	public class UIGlowingLine : MonoBehaviour
	{
		public VolumetricLineBehavior[] Lines;
		public GameObject EndObject;
		public float Speed;
		public ParticleSystem Particles;

		private int _currentIndex = 0;
		private bool _completed = false;
		private Vector3 _currentPoint;
		private Vector3 _startingPoint;
		private Vector3 _nextPoint;
		private float _startTime;
		private float _journeyLength;
		private VolumetricLineBehavior _currentLine;


		// Use this for initialization
		protected void Start()
		{
			if (Lines.Length > 0)
			{
				foreach (var volumetricLineBehavior in Lines)
				{
					volumetricLineBehavior.gameObject.SetActive(false);
				}
				EndObject.SetActive(false);
				SetNextPoint();
				
				Particles.transform.localPosition = _currentPoint;
				Particles.gameObject.SetActive(true);
			}
			else
			{
				EndObject.SetActive(true);
				_completed = true;
			}
		}

		// Update is called once per frame
		protected void Update()
		{
			if (_completed) return;

			float distanceCovered = (Time.time - _startTime) * Speed;
			float journeyFraction = distanceCovered / _journeyLength;
			var currentPoint = Vector3.Lerp(_currentPoint, _nextPoint, journeyFraction);

			_currentPoint = currentPoint;
			
			Particles.transform.localPosition = _currentPoint;
			_currentLine.SetStartAndEndPoints(_startingPoint, _currentPoint);

			if (currentPoint == _nextPoint)
			{
				_currentIndex++;
				CheckCompleted();
				if (_completed) return;
				SetNextPoint();
			}
		}

		protected void SetNextPoint()
		{
			_currentLine = Lines[_currentIndex];
			_nextPoint = _currentLine.EndPos;
			_currentPoint = _currentLine.StartPos;
			_startingPoint = _currentLine.StartPos;
			_currentLine.SetStartAndEndPoints(_currentLine.StartPos, _currentPoint);

			CalculateJourneyData();

			_currentLine.gameObject.SetActive(true);
		}

		protected void CalculateJourneyData()
		{
			_startTime = Time.time;
			_journeyLength = Vector3.Distance(_currentPoint, _nextPoint);
		}

		protected void CheckCompleted()
		{
			if (_currentIndex >= Lines.Length)
			{
				EndObject.SetActive(true);
				Particles.gameObject.SetActive(false);
				_completed = true;
			}
		}
	}
}