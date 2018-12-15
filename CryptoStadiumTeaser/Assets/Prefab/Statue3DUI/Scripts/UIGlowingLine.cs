using UnityEngine;
using VolumetricLines;

namespace NFLCryptoFootball.UI
{
	public class UIGlowingLine : MonoBehaviour
	{
		public Transform[] Points;
		public VolumetricLineBehavior Line;
		public GameObject EndObject;
		public float Speed;
		public ParticleSystem Particles;

		//private List<Vector3> _points = new List<Vector3>(3);
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
			if (Points.Length > 1)
			{
				//_points.Add(Points[_currentIndex].position);
				SetNextPoint();

				//Line.UpdateLineVertices(_points.ToArray());
				Particles.transform.position = _currentPoint;
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
			CheckCompleted();
			if (_completed) return;

			float distanceCovered = (Time.time - _startTime) * Speed;
			float journeyFraction = distanceCovered / _journeyLength;
			var currentPoint = Vector3.Lerp(_currentPoint, _nextPoint, journeyFraction);

			_currentPoint = currentPoint;

			//Line.UpdateLineVertices(_points.ToArray());
			Particles.transform.position = _currentPoint;
			_currentLine.SetStartAndEndPoints(_startingPoint, _currentPoint);

			if (currentPoint == Points[_currentIndex].position)
			{
				SetNextPoint();
			}
		}

		protected void SetNextPoint()
		{
			_currentIndex++;
			if (_currentIndex >= Points.Length)
			{
				return;
			}

			_currentPoint = Points[_currentIndex - 1].position;
			_startingPoint = _currentPoint;
			_nextPoint = Points[_currentIndex].position;

			_startTime = Time.time;
			_journeyLength = Vector3.Distance(_currentPoint, _nextPoint);

			_currentLine = GameObject.Instantiate(Line.gameObject, this.transform).GetComponent<VolumetricLineBehavior>();
			_currentLine.SetStartAndEndPoints(_startingPoint, _currentPoint);
			_currentLine.gameObject.SetActive(true);
		}

		protected void CheckCompleted()
		{
			if (_currentIndex >= Points.Length)
			{
				EndObject.SetActive(true);
				Particles.gameObject.SetActive(false);
				_completed = true;
			}
		}
	}
}