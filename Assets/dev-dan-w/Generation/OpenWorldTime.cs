using System.Collections;
using UnityEngine;
using System;

namespace WorldTime
{
    public class OpenWorldTime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;

        [SerializeField] private float _dayLength = 1; // How long a day is in seconds
        private TimeSpan _currentTime = new TimeSpan();
        private bool stopTime = true;
        private float _minuteLength => _dayLength / WorldTimeConstants.MinutesInDay;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(AddMinute());
        }
        private IEnumerator AddMinute()
        {
            if(!stopTime)
            {
                _currentTime += TimeSpan.FromMinutes(1);
            }
            WorldTimeChanged?.Invoke(this, _currentTime);
            if (LoadFloorType()) stopTime = false;
            else stopTime = true;

            yield return new WaitForSeconds(_minuteLength);
            StartCoroutine(AddMinute());
        }
        
        public bool LoadFloorType()
        {
            // Get the saved value from PlayerPrefs, defaulting to 0 if not found
            int floorTypeValue = PlayerPrefs.GetInt("floorType", 0);
            // Set the floor type based on the saved value
            bool isDungeonFloor = floorTypeValue != 1 ? true : false;

            return isDungeonFloor;
        }

    }
}
