using System.Collections.Generic;

namespace VedAstro.Library
{
    public enum BirdName
    {
        Vulture,
        Owl,
        Crow,
        Cock,
        Peacock
    }

    public enum BirdActivity
    {
        /// <summary>
        /// Thus, the Dying and Sleeping states
        /// are very weak and unsuitable for any action
        /// </summary>
        Dying,

        /// <summary>
        /// 
        /// </summary>
        Sleeping,

        Walking,

        /// <summary>
        /// Thus, the Eating and Ruling activity periods will be
        /// suitable for all the actions of day to day life to consummate
        /// into success.
        /// </summary>
        Eating,

        /// <summary>
        /// Thus, the Eating and Ruling activity periods will be
        /// suitable for all the actions of day to day life to consummate
        /// into success.
        /// </summary>
        Ruling
    }

    public enum BirthTimeInVedicDay
    {
        PreviousDay, Yes, NextDay
    }


    /// <summary>
    /// Deeper data and logic for Pancha Pakshi used by Calculate API
    /// </summary>
    public class PanchaPakshi
    {
        /// <summary>
        /// Table Showing the Strength of Abstract Activities
        /// for the 5 Birds
        /// 
        /// Pre-calculated Pancha Pakshi table from pg 48
        /// BIORHYTHMS OF NATAL MOON : (MYSTERIES OF PANCH PAKSHI) BY Prof. Dr. U.S. Pulippani :
        /// A TREATISE OF FIVE ELEMENTS (TATVAS) FEATHERED IN A NATIVE
        /// </summary>
        public static Dictionary<BirdName, Dictionary<BirdActivity, Dictionary<BirdActivity, double>>> AbstractActivityStrengthTable = new()
            {
                {
                    BirdName.Crow, new Dictionary<BirdActivity, Dictionary<BirdActivity, double>>
                    {
                        {
                            BirdActivity.Ruling, new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling ,1.00},
                                {BirdActivity.Eating, 0.80},
                                {BirdActivity.Walking, 0.60},
                                {BirdActivity.Sleeping, 0.40},
                                {BirdActivity.Dying, 0.20}
                            }
                        },
                        {
                            BirdActivity.Eating,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.8},
                                {BirdActivity.Eating, 0.64},
                                {BirdActivity.Walking, 0.48},
                                {BirdActivity.Sleeping, 0.32},
                                {BirdActivity.Dying, 0.16}
                            }
                        },
                        {
                            BirdActivity.Walking,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling, 0.6},
                                {BirdActivity.Eating, 0.48},
                                {BirdActivity.Walking, 0.36},
                                {BirdActivity.Sleeping, 0.24},
                                {BirdActivity.Dying, 0.12}
                            }
                        },
                        {
                            BirdActivity.Sleeping,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.4},
                                {BirdActivity.Eating, 0.32},
                                {BirdActivity.Walking, 0.24},
                                {BirdActivity.Sleeping, 0.16},
                                {BirdActivity.Dying, 0.08}
                            }
                        },
                        {
                            BirdActivity.Dying,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.2},
                                {BirdActivity.Eating, 0.16},
                                {BirdActivity.Walking, 0.12},
                                {BirdActivity.Sleeping, 0.08},
                                {BirdActivity.Dying, 0.04}
                            }
                        }
                    }
                },
                {
                    BirdName.Vulture, new Dictionary<BirdActivity, Dictionary<BirdActivity, double>>
                    {
                        {
                            BirdActivity.Ruling, new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling ,0.75},
                                {BirdActivity.Eating, 0.60},
                                {BirdActivity.Walking, 0.45},
                                {BirdActivity.Sleeping, 0.30},
                                {BirdActivity.Dying, 0.15}
                            }
                        },
                        {
                            BirdActivity.Eating,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.60},
                                {BirdActivity.Eating, 0.48},
                                {BirdActivity.Walking, 0.36},
                                {BirdActivity.Sleeping, 0.24},
                                {BirdActivity.Dying, 0.12}
                            }
                        },
                        {
                            BirdActivity.Walking,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.45},
                                {BirdActivity.Eating , 0.36},
                                {BirdActivity.Walking , 0.27},
                                {BirdActivity.Sleeping ,0.18},
                                {BirdActivity.Dying ,0.09}
                            }
                        },
                        {
                            BirdActivity.Sleeping,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.30},
                                {BirdActivity.Eating , 0.24},
                                {BirdActivity.Walking , 0.18},
                                {BirdActivity.Sleeping ,0.12},
                                {BirdActivity.Dying ,0.06}
                            }
                        },
                        {
                            BirdActivity.Dying,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.15},
                                {BirdActivity.Eating , 0.12},
                                {BirdActivity.Walking , 0.09},
                                {BirdActivity.Sleeping ,0.06},
                                {BirdActivity.Dying ,0.03}
                            }
                        }
                    }
                },
                {
                    BirdName.Owl, new Dictionary<BirdActivity, Dictionary<BirdActivity, double>>
                    {
                        {
                            BirdActivity.Ruling, new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling ,0.50},
                                {BirdActivity.Eating, 0.40},
                                {BirdActivity.Walking, 0.30},
                                {BirdActivity.Sleeping, 0.20},
                                {BirdActivity.Dying, 0.10}
                            }
                        },
                        {
                            BirdActivity.Eating,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.40},
                                {BirdActivity.Eating, 0.32},
                                {BirdActivity.Walking, 0.24},
                                {BirdActivity.Sleeping, 0.16},
                                {BirdActivity.Dying, 0.08}
                            }
                        },
                        {
                            BirdActivity.Walking,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.30},
                                {BirdActivity.Eating, 0.24},
                                {BirdActivity.Walking , 0.18},
                                {BirdActivity.Sleeping , 0.12},
                                {BirdActivity.Dying , 0.06}
                            }
                        },
                        {
                            BirdActivity.Sleeping,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.20},
                                {BirdActivity.Eating, 0.16},
                                {BirdActivity.Walking, 0.12},
                                {BirdActivity.Sleeping, 0.08},
                                {BirdActivity.Dying, 0.04}
                            }
                        },
                        {
                            BirdActivity.Dying,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.10},
                                {BirdActivity.Eating, 0.08},
                                {BirdActivity.Walking, 0.06},
                                {BirdActivity.Sleeping, 0.04},
                                {BirdActivity.Dying, 0.02}
                            }
                        }
                    }
                },
                {
                    BirdName.Cock, new Dictionary<BirdActivity, Dictionary<BirdActivity, double>>
                    {
                        {
                            BirdActivity.Ruling, new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling ,0.25},
                                {BirdActivity.Eating, 0.20},
                                {BirdActivity.Walking, 0.15},
                                {BirdActivity.Sleeping, 0.10},
                                {BirdActivity.Dying, 0.05}
                            }
                        },
                        {
                            BirdActivity.Eating,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.20},
                                {BirdActivity.Eating, 0.16},
                                {BirdActivity.Walking, 0.12},
                                {BirdActivity.Sleeping, 0.08},
                                {BirdActivity.Dying, 0.04}
                            }
                        },
                        {
                            BirdActivity.Walking,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.15},
                                {BirdActivity.Eating, 0.12},
                                {BirdActivity.Walking , 0.09},
                                {BirdActivity.Sleeping , .06},
                                {BirdActivity.Dying , .03}
                            }
                        },
                        {
                            BirdActivity.Sleeping,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.10},
                                {BirdActivity.Eating, 0.08},
                                {BirdActivity.Walking, 0.06},
                                {BirdActivity.Sleeping, 0.04},
                                {BirdActivity.Dying, 0.02}
                            }
                        },
                        {
                            BirdActivity.Dying,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.05},
                                {BirdActivity.Eating, 0.04},
                                {BirdActivity.Walking, 0.03},
                                {BirdActivity.Sleeping, 0.02},
                                {BirdActivity.Dying, 0.01}
                            }
                        }
                    }
                },
                {
                    BirdName.Peacock, new Dictionary<BirdActivity, Dictionary<BirdActivity, double>>
                    {
                        {
                            BirdActivity.Ruling, new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling ,0.125},
                                {BirdActivity.Eating, 0.10},
                                {BirdActivity.Walking, 0.075},
                                {BirdActivity.Sleeping, 0.05},
                                {BirdActivity.Dying, 0.025}
                            }
                        },
                        {
                            BirdActivity.Eating,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.10},
                                {BirdActivity.Eating, 0.08},
                                {BirdActivity.Walking, 0.06},
                                {BirdActivity.Sleeping, 0.04},
                                {BirdActivity.Dying, 0.02}
                            }
                        },
                        {
                            BirdActivity.Walking,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.075},
                                {BirdActivity.Eating, 0.06},
                                {BirdActivity.Walking , 0.045},
                                {BirdActivity.Sleeping , 0.03 },
                                {BirdActivity.Dying , 0.015}
                            }
                        },
                        {
                            BirdActivity.Sleeping,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.05},
                                {BirdActivity.Eating, 0.04},
                                {BirdActivity.Walking, 0.03},
                                {BirdActivity.Sleeping, 0.02},
                                {BirdActivity.Dying, 0.01}
                            }
                        },
                        {
                            BirdActivity.Dying,new Dictionary<BirdActivity, double>
                            {
                                {BirdActivity.Ruling , 0.025},
                                {BirdActivity.Eating, 0.02},
                                {BirdActivity.Walking, 0.015},
                                {BirdActivity.Sleeping, 0.01},
                                {BirdActivity.Dying, 0.005}
                            }
                        }
                    }
                }
            };

    }
}
