using System;
using System.Collections.Generic;

namespace Visualizer
{
    [Serializable]
    public class PlayerFrame
    {
        public float time;
        public float[] position;
        public float[] rotation;
    }

    [Serializable]
    public class PlayerData
    {
        public List<PlayerFrame> frames;
    }

    [Serializable]
    public class FootballFrame
    {
        public float time;
        public float[] position;
        public float[] rotation;
    }

    [Serializable]
    public class Metadata
    {
        public string playName;
        public float duration;
        public string description;
    }

    [Serializable]
    public class PlayData
    {
        public Metadata metadata;
        public List<PlayerFrame> qb;
        public List<PlayerFrame> wr;
        public List<PlayerFrame> rb;
        public List<FootballFrame> football;
    }
}
