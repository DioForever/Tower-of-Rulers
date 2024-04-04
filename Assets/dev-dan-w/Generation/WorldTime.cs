using System.Collections;
using UnityEngine;
using System;

namespace WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField] private float _dayLength = 1;
        private TimeSpan _currentTime = new TimeSpan();
        private bool stopTime = true;
        private bool initLight = false;
        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(AddMinute());
        }

        private IEnumerator AddMinute()
        {
            if (!stopTime)
            {
                _currentTime += TimeSpan.FromMinutes(1);
            }
            if (!initLight)
            {
                WorldTimeChanged?.Invoke(this, _currentTime);
                initLight = true;
            }

            if (LoadFloorType()) stopTime = false;
            else stopTime = true;

            if (!stopTime) WorldTimeChanged?.Invoke(this, _currentTime);

            yield return new WaitForSeconds(_minuteLength);
            StartCoroutine(AddMinute());
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
