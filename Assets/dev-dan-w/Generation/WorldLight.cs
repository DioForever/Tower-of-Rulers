using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace WorldTime
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        private Light2D _light;

        [SerializeField] private WorldTime _worldTime;
        [SerializeField] private Gradient _gradiant;
        [SerializeField] private Gradient _gradiantDungeon;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            if (LoadFloorType()) _light.color = _gradiant.Evaluate(PercentOfDay(newTime));
            else _light.color = _gradiantDungeon.Evaluate(PercentOfDay(newTime));
        }

        private float PercentOfDay(TimeSpan time)
        {
            return (float)time.TotalMinutes % WorldTimeConstants.MinutesInDay / WorldTimeConstants.MinutesInDay;
        }

        public bool LoadFloorType()
        {
            // Get the saved value from PlayerPrefs, defaulting to 0 if not found
            int floorTypeValue = PlayerPrefs.GetInt("floorType", 0);
            // Set the floor type based on the saved value
            bool isDungeonFloor = floorTypeValue != 1 ? true : false;
            // dungeon floor is flase, overworld floor is true

            return isDungeonFloor;
        }
    }
}
