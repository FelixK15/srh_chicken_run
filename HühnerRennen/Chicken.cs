﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.IO;

namespace HühnerRennen
{
    class Chicken
    {
        private static Random r = new Random();
        private double m_curAnimationTime = 0.0;

        private static double m_animationTime = 7.0;

        private Image[] m_animation = new Image[2];
        private int m_curAnimation = 0;

        public enum ChickenType
        {
            GREEN_CHICKEN = 0,
            RED_CHICKEN,
            BLUE_CHICKEN,
            VIOLET_CHICKEN
        }

        public string Name
        {
            get;
            set;
        }

        public Image Graphic
        {
            get { return m_animation[m_curAnimation]; }
        }

        public double Speed
        {
            get;
            set;
        }

        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }

        public int WonRaces
        {
            get;
            set;
        }

        public int LostRaces
        {
            get;
            set;
        }

        public double BidMoney
        {
            get;
            set;
        }

        public Chicken(ChickenType type)
        {
            m_curAnimationTime = m_animationTime;

            m_animation[0] = new Image();
            m_animation[1] = new Image();

            _LoadAnimation(type);
        }

        public void CalculateSpeed()
        {
            Speed = r.Next(10);

            m_curAnimationTime -= Speed;

            //Wechsle die Animation.
            if (m_curAnimationTime < 0.0)
            {
                m_curAnimationTime = m_animationTime;
                m_curAnimation = m_curAnimation == 0 ? 1 : 0;
            }

            X += (int)(Speed * r.NextDouble());
        }

        private void _LoadAnimation(ChickenType type)
        {
            if (type == ChickenType.BLUE_CHICKEN)
            {
                m_animation[0].Source = ChickenResources.BlueChicken_1;
                m_animation[1].Source = ChickenResources.BlueChicken_2;
            }
            else if (type == ChickenType.GREEN_CHICKEN)
            {
                m_animation[0].Source = ChickenResources.GreenChicken_1;
                m_animation[1].Source = ChickenResources.GreenChicken_2;
            }
            else if (type == ChickenType.RED_CHICKEN)
            {
                m_animation[0].Source = ChickenResources.RedChicken_1;
                m_animation[1].Source = ChickenResources.RedChicken_2;
            }
            else
            {
                m_animation[0].Source = ChickenResources.VioletChicken_1;
                m_animation[1].Source = ChickenResources.VioletChicken_2;
            }
        }
    }
}
