using KillChain.Core.Bootstrap;
using UnityEngine;

namespace KillChain.Debug
{
    [AutoBootstrap]
    public class DebugFPSText : DebugText
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;

        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        }


        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
            }
        }

        private void OnGUI()
        {
            _guiStyle.alignment = TextAnchor.UpperRight;
            GUI.Label(new Rect(Screen.width - 25, 5, 20, 20), $"{m_CurrentFps}", _guiStyle);
        }
    }
}

