using System;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace FreedLOW.MLAgents.Sensors
{
    public class SphereSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private int maximumObservations = 10;
        
        private const int ObservationSize = 3;

        private IList<Vector3> _observations;
        
        private void Start()
        {
            _observations = new List<Vector3>(maximumObservations);
        }

        public ObservationSpec GetObservationSpec()
        {
            return ObservationSpec.VariableLength(ObservationSize, maximumObservations);
        }

        public int Write(ObservationWriter writer)
        {
            var observationDeficit = maximumObservations - _observations.Count;
            var observationCount = _observations.Count;

            for (int i = 0; i < observationCount; i++)
            {
                var offset = i * ObservationSize;
                writer.Add(_observations[i], offset);
            }

            for (int i = observationCount; i < observationDeficit; i++)
            {
                var offset = i * ObservationSize;
                writer.Add(_observations[i], offset);
            }

            return ObservationSize * maximumObservations;
        }

        public byte[] GetCompressedObservation()
        {
            return null;
        }

        public void Update()
        {
            
        }

        public void Reset()
        {
            
        }

        public CompressionSpec GetCompressionSpec()
        {
            return CompressionSpec.Default();
        }

        public string GetName()
        {
            return "SphereSensor";
        }
    }
}