using System;
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

    public record BirthYama(int YamaCount, Time YamaStartTime, Time YamaEndTime);


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

        public enum TimeOfDay
        {
            Night, Day
        }

        //levels Day/Night Time > Day of week > Yama number > Bird > Activity
        public static Dictionary<TimeOfDay, Dictionary<DayOfWeek, Dictionary<int, Dictionary<BirdName, BirdActivity>>>>
            TableData = new()
            {
                {
                    TimeOfDay.Day, new Dictionary<DayOfWeek, Dictionary<int,  Dictionary<BirdName, BirdActivity>>>
                    {
                        {
                            DayOfWeek.Sunday, new Dictionary<int,  Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Vulture, BirdActivity.Eating},
                                        {BirdName.Owl,BirdActivity.Walking},
                                        {BirdName.Crow,BirdActivity.Ruling},
                                        {BirdName.Cock,BirdActivity.Sleeping},
                                        {BirdName.Peacock,BirdActivity.Dying}
                                    }
                                },
                                {
                                    2,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Peacock,BirdActivity.Eating},
                                        {BirdName.Vulture,BirdActivity.Walking},
                                        {BirdName.Owl,BirdActivity.Ruling},
                                        {BirdName.Crow,BirdActivity.Sleeping},
                                        {BirdName.Cock,BirdActivity.Dying}
                                    }
                                },
                                {
                                    3,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Cock , BirdActivity.Eating},
                                        {BirdName.Peacock , BirdActivity.Walking},
                                        {BirdName.Vulture , BirdActivity.Ruling},
                                        {BirdName.Owl , BirdActivity.Sleeping},
                                        {BirdName.Crow , BirdActivity.Dying}
                                    }
                                },
                                {
                                    4,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Crow , BirdActivity.Eating},
                                        {BirdName.Cock , BirdActivity.Walking},
                                        {BirdName.Peacock , BirdActivity.Ruling},
                                        {BirdName.Vulture , BirdActivity.Sleeping},
                                        {BirdName.Owl , BirdActivity.Dying}
                                    }
                                },
                                {
                                    5,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Owl , BirdActivity.Eating},
                                        {BirdName.Crow , BirdActivity.Walking},
                                        {BirdName.Cock , BirdActivity.Ruling},
                                        {BirdName.Peacock , BirdActivity.Sleeping},
                                        {BirdName.Vulture , BirdActivity.Dying}
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Tuesday, new Dictionary<int,  Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Vulture, BirdActivity.Eating},
                                        {BirdName.Owl,BirdActivity.Walking},
                                        {BirdName.Crow,BirdActivity.Ruling},
                                        {BirdName.Cock,BirdActivity.Sleeping},
                                        {BirdName.Peacock,BirdActivity.Dying}
                                    }
                                },
                                {
                                    2,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Peacock,BirdActivity.Eating},
                                        {BirdName.Vulture,BirdActivity.Walking},
                                        {BirdName.Owl,BirdActivity.Ruling},
                                        {BirdName.Crow,BirdActivity.Sleeping},
                                        {BirdName.Cock,BirdActivity.Dying}
                                    }
                                },
                                {
                                    3,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Cock , BirdActivity.Eating},
                                        {BirdName.Peacock , BirdActivity.Walking},
                                        {BirdName.Vulture , BirdActivity.Ruling},
                                        {BirdName.Owl , BirdActivity.Sleeping},
                                        {BirdName.Crow , BirdActivity.Dying}
                                    }
                                },
                                {
                                    4,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Crow , BirdActivity.Eating},
                                        {BirdName.Cock , BirdActivity.Walking},
                                        {BirdName.Peacock , BirdActivity.Ruling},
                                        {BirdName.Vulture , BirdActivity.Sleeping},
                                        {BirdName.Owl , BirdActivity.Dying}
                                    }
                                },
                                {
                                    5,new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Owl , BirdActivity.Eating},
                                        {BirdName.Crow , BirdActivity.Walking},
                                        {BirdName.Cock , BirdActivity.Ruling},
                                        {BirdName.Peacock , BirdActivity.Sleeping},
                                        {BirdName.Vulture , BirdActivity.Dying}
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Monday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Owl, BirdActivity.Eating},
                                        {BirdName.Crow, BirdActivity.Walking},
                                        {BirdName.Cock, BirdActivity.Ruling},
                                        {BirdName.Peacock, BirdActivity.Sleeping},
                                        {BirdName.Vulture, BirdActivity.Dying}
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Vulture, BirdActivity.Eating},
                                        {BirdName.Owl, BirdActivity.Walking},
                                        {BirdName.Crow, BirdActivity.Ruling},
                                        {BirdName.Cock, BirdActivity.Sleeping},
                                        {BirdName.Peacock, BirdActivity.Dying}
                                    }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Peacock, BirdActivity.Eating},
                                        {BirdName.Vulture, BirdActivity.Walking},
                                        {BirdName.Owl, BirdActivity.Ruling},
                                        {BirdName.Crow, BirdActivity.Sleeping},
                                        {BirdName.Cock, BirdActivity.Dying}
                                    }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Cock, BirdActivity.Eating},
                                        {BirdName.Peacock, BirdActivity.Walking},
                                        {BirdName.Vulture, BirdActivity.Ruling},
                                        {BirdName.Owl, BirdActivity.Sleeping},
                                        {BirdName.Crow, BirdActivity.Dying}
                                    }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Crow, BirdActivity.Eating},
                                        {BirdName.Cock, BirdActivity.Walking},
                                        {BirdName.Peacock, BirdActivity.Ruling},
                                        {BirdName.Vulture, BirdActivity.Sleeping},
                                        {BirdName.Owl, BirdActivity.Dying}
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Wednesday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Owl, BirdActivity.Eating},
                                        {BirdName.Crow, BirdActivity.Walking},
                                        {BirdName.Cock, BirdActivity.Ruling},
                                        {BirdName.Peacock, BirdActivity.Sleeping},
                                        {BirdName.Vulture, BirdActivity.Dying}
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Vulture, BirdActivity.Eating},
                                        {BirdName.Owl, BirdActivity.Walking},
                                        {BirdName.Crow, BirdActivity.Ruling},
                                        {BirdName.Cock, BirdActivity.Sleeping},
                                        {BirdName.Peacock, BirdActivity.Dying}
                                    }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Peacock, BirdActivity.Eating},
                                        {BirdName.Vulture, BirdActivity.Walking},
                                        {BirdName.Owl, BirdActivity.Ruling},
                                        {BirdName.Crow, BirdActivity.Sleeping},
                                        {BirdName.Cock, BirdActivity.Dying}
                                    }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Cock, BirdActivity.Eating},
                                        {BirdName.Peacock, BirdActivity.Walking},
                                        {BirdName.Vulture, BirdActivity.Ruling},
                                        {BirdName.Owl, BirdActivity.Sleeping},
                                        {BirdName.Crow, BirdActivity.Dying}
                                    }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Crow, BirdActivity.Eating},
                                        {BirdName.Cock, BirdActivity.Walking},
                                        {BirdName.Peacock, BirdActivity.Ruling},
                                        {BirdName.Vulture, BirdActivity.Sleeping},
                                        {BirdName.Owl, BirdActivity.Dying}
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Thursday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Crow, BirdActivity.Eating },
                                        { BirdName.Cock, BirdActivity.Walking },
                                        { BirdName.Peacock, BirdActivity.Ruling },
                                        { BirdName.Vulture, BirdActivity.Sleeping },
                                        { BirdName.Owl, BirdActivity.Dying }
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Owl, BirdActivity.Eating },
                                        { BirdName.Crow, BirdActivity.Walking },
                                        { BirdName.Cock, BirdActivity.Ruling },
                                        { BirdName.Peacock, BirdActivity.Sleeping },
                                        { BirdName.Vulture, BirdActivity.Dying }
                                    }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Vulture, BirdActivity.Eating },
                                        { BirdName.Owl, BirdActivity.Walking },
                                        { BirdName.Crow, BirdActivity.Ruling },
                                        { BirdName.Cock, BirdActivity.Sleeping },
                                        { BirdName.Peacock, BirdActivity.Dying }
                                    }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Peacock, BirdActivity.Eating },
                                        { BirdName.Vulture, BirdActivity.Walking },
                                        { BirdName.Owl, BirdActivity.Ruling },
                                        { BirdName.Crow, BirdActivity.Sleeping },
                                        { BirdName.Cock, BirdActivity.Dying }
                                    }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Cock, BirdActivity.Eating },
                                        { BirdName.Peacock, BirdActivity.Walking },
                                        { BirdName.Vulture, BirdActivity.Ruling },
                                        { BirdName.Owl, BirdActivity.Sleeping },
                                        { BirdName.Crow, BirdActivity.Dying }
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Friday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Cock, BirdActivity.Eating },
                                        { BirdName.Peacock, BirdActivity.Walking },
                                        { BirdName.Vulture, BirdActivity.Ruling },
                                        { BirdName.Owl, BirdActivity.Sleeping },
                                        { BirdName.Crow, BirdActivity.Dying }
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Crow, BirdActivity.Eating },
                                        { BirdName.Cock, BirdActivity.Walking },
                                        { BirdName.Peacock, BirdActivity.Ruling },
                                        { BirdName.Vulture, BirdActivity.Sleeping },
                                        { BirdName.Owl, BirdActivity.Dying }
                                    }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Owl, BirdActivity.Eating },
                                        { BirdName.Crow, BirdActivity.Walking },
                                        { BirdName.Cock, BirdActivity.Ruling },
                                        { BirdName.Peacock, BirdActivity.Sleeping },
                                        { BirdName.Vulture, BirdActivity.Dying }
                                    }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Vulture, BirdActivity.Eating },
                                        { BirdName.Owl, BirdActivity.Walking },
                                        { BirdName.Crow, BirdActivity.Ruling },
                                        { BirdName.Cock, BirdActivity.Sleeping },
                                        { BirdName.Peacock, BirdActivity.Dying }
                                    }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                    {
                                        { BirdName.Peacock, BirdActivity.Eating },
                                        { BirdName.Vulture, BirdActivity.Walking },
                                        { BirdName.Owl, BirdActivity.Ruling },
                                        { BirdName.Crow, BirdActivity.Sleeping },
                                        { BirdName.Cock, BirdActivity.Dying }
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Saturday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Peacock, BirdActivity.Eating},
                                        {BirdName.Vulture, BirdActivity.Walking},
                                        {BirdName.Owl, BirdActivity.Ruling},
                                        {BirdName.Crow, BirdActivity.Sleeping},
                                        {BirdName.Cock, BirdActivity.Dying}
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Cock, BirdActivity.Eating},
                                        {BirdName.Peacock, BirdActivity.Walking},
                                        {BirdName.Vulture, BirdActivity.Ruling},
                                        {BirdName.Owl, BirdActivity.Sleeping},
                                        {BirdName.Crow, BirdActivity.Dying}
                                    }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Crow, BirdActivity.Eating},
                                        {BirdName.Cock, BirdActivity.Walking},
                                        {BirdName.Peacock, BirdActivity.Ruling},
                                        {BirdName.Vulture, BirdActivity.Sleeping},
                                        {BirdName.Owl, BirdActivity.Dying}
                                    }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Owl, BirdActivity.Eating},
                                        {BirdName.Crow, BirdActivity.Walking},
                                        {BirdName.Cock, BirdActivity.Ruling},
                                        {BirdName.Peacock, BirdActivity.Sleeping},
                                        {BirdName.Vulture, BirdActivity.Dying}
                                    }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Vulture, BirdActivity.Eating},
                                        {BirdName.Owl, BirdActivity.Walking},
                                        {BirdName.Crow, BirdActivity.Ruling},
                                        {BirdName.Cock, BirdActivity.Sleeping},
                                        {BirdName.Peacock, BirdActivity.Dying}
                                    }
                                }
                            }
                        }
                    }
                },
                {
                    TimeOfDay.Night, new Dictionary<DayOfWeek, Dictionary<int,  Dictionary<BirdName, BirdActivity>>>
                    {
                        {
                            DayOfWeek.Sunday, new Dictionary<int, Dictionary<BirdName,BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Crow,BirdActivity.Eating},
                                        {BirdName.Owl,BirdActivity.Ruling},
                                        {BirdName.Vulture,BirdActivity.Dying},
                                        {BirdName.Peacock,BirdActivity.Walking},
                                        {BirdName.Cock,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    2,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Cock,BirdActivity.Eating},
                                        {BirdName.Crow,BirdActivity.Ruling},
                                        {BirdName.Owl,BirdActivity.Dying},
                                        {BirdName.Vulture,BirdActivity.Walking},
                                        {BirdName.Peacock,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    3,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Peacock,BirdActivity.Eating},
                                        {BirdName.Cock,BirdActivity.Ruling},
                                        {BirdName.Crow,BirdActivity.Dying},
                                        {BirdName.Owl,BirdActivity.Walking},
                                        {BirdName.Vulture,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    4,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Vulture,BirdActivity.Eating},
                                        {BirdName.Peacock,BirdActivity.Ruling},
                                        {BirdName.Cock,BirdActivity.Dying},
                                        {BirdName.Crow,BirdActivity.Walking},
                                        {BirdName.Owl,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    5,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Owl,BirdActivity.Eating},
                                        {BirdName.Vulture,BirdActivity.Ruling},
                                        {BirdName.Peacock,BirdActivity.Dying},
                                        {BirdName.Cock,BirdActivity.Walking},
                                        {BirdName.Crow,BirdActivity.Sleeping}
                                    }
                                },
                            }
                        },
                        {
                            DayOfWeek.Tuesday, new Dictionary<int, Dictionary<BirdName,BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Crow,BirdActivity.Eating},
                                        {BirdName.Owl,BirdActivity.Ruling},
                                        {BirdName.Vulture,BirdActivity.Dying},
                                        {BirdName.Peacock,BirdActivity.Walking},
                                        {BirdName.Cock,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    2,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Cock,BirdActivity.Eating},
                                        {BirdName.Crow,BirdActivity.Ruling},
                                        {BirdName.Owl,BirdActivity.Dying},
                                        {BirdName.Vulture,BirdActivity.Walking},
                                        {BirdName.Peacock,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    3,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Peacock,BirdActivity.Eating},
                                        {BirdName.Cock,BirdActivity.Ruling},
                                        {BirdName.Crow,BirdActivity.Dying},
                                        {BirdName.Owl,BirdActivity.Walking},
                                        {BirdName.Vulture,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    4,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Vulture,BirdActivity.Eating},
                                        {BirdName.Peacock,BirdActivity.Ruling},
                                        {BirdName.Cock,BirdActivity.Dying},
                                        {BirdName.Crow,BirdActivity.Walking},
                                        {BirdName.Owl,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    5,
                                    new Dictionary<BirdName,BirdActivity>
                                    {
                                        {BirdName.Owl,BirdActivity.Eating},
                                        {BirdName.Vulture,BirdActivity.Ruling},
                                        {BirdName.Peacock,BirdActivity.Dying},
                                        {BirdName.Cock,BirdActivity.Walking},
                                        {BirdName.Crow,BirdActivity.Sleeping}
                                    }
                                },
                            }
                        },
                        {
                            DayOfWeek.Monday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                           {
                               {
                                   1, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Cock, BirdActivity.Eating},
                                       {BirdName.Crow, BirdActivity.Ruling},
                                       {BirdName.Owl, BirdActivity.Dying},
                                       {BirdName.Vulture, BirdActivity.Walking},
                                       {BirdName.Peacock, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   2, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Peacock, BirdActivity.Eating},
                                       {BirdName.Cock, BirdActivity.Ruling},
                                       {BirdName.Crow, BirdActivity.Dying},
                                       {BirdName.Owl, BirdActivity.Walking},
                                       {BirdName.Vulture, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   3, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Vulture, BirdActivity.Eating},
                                       {BirdName.Peacock, BirdActivity.Ruling},
                                       {BirdName.Cock, BirdActivity.Dying},
                                       {BirdName.Crow, BirdActivity.Walking},
                                       {BirdName.Owl, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   4, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Owl, BirdActivity.Eating},
                                       {BirdName.Vulture, BirdActivity.Ruling},
                                       {BirdName.Peacock, BirdActivity.Dying},
                                       {BirdName.Cock, BirdActivity.Walking},
                                       {BirdName.Crow, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   5, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Crow, BirdActivity.Eating},
                                       {BirdName.Owl, BirdActivity.Ruling},
                                       {BirdName.Vulture, BirdActivity.Dying},
                                       {BirdName.Peacock, BirdActivity.Walking},
                                       {BirdName.Cock, BirdActivity.Sleeping}
                                   }
                               }
                           }
                        },
                        {
                            DayOfWeek.Wednesday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                           {
                               {
                                   1, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Cock, BirdActivity.Eating},
                                       {BirdName.Crow, BirdActivity.Ruling},
                                       {BirdName.Owl, BirdActivity.Dying},
                                       {BirdName.Vulture, BirdActivity.Walking},
                                       {BirdName.Peacock, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   2, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Peacock, BirdActivity.Eating},
                                       {BirdName.Cock, BirdActivity.Ruling},
                                       {BirdName.Crow, BirdActivity.Dying},
                                       {BirdName.Owl, BirdActivity.Walking},
                                       {BirdName.Vulture, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   3, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Vulture, BirdActivity.Eating},
                                       {BirdName.Peacock, BirdActivity.Ruling},
                                       {BirdName.Cock, BirdActivity.Dying},
                                       {BirdName.Crow, BirdActivity.Walking},
                                       {BirdName.Owl, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   4, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Owl, BirdActivity.Eating},
                                       {BirdName.Vulture, BirdActivity.Ruling},
                                       {BirdName.Peacock, BirdActivity.Dying},
                                       {BirdName.Cock, BirdActivity.Walking},
                                       {BirdName.Crow, BirdActivity.Sleeping}
                                   }
                               },
                               {
                                   5, new Dictionary<BirdName, BirdActivity>
                                   {
                                       {BirdName.Crow, BirdActivity.Eating},
                                       {BirdName.Owl, BirdActivity.Ruling},
                                       {BirdName.Vulture, BirdActivity.Dying},
                                       {BirdName.Peacock, BirdActivity.Walking},
                                       {BirdName.Cock, BirdActivity.Sleeping}
                                   }
                               }
                           }
                        },
                        {
                            DayOfWeek.Thursday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Peacock, BirdActivity.Eating},
                                        {BirdName.Cock, BirdActivity.Ruling},
                                        {BirdName.Crow, BirdActivity.Dying},
                                        {BirdName.Owl, BirdActivity.Walking},
                                        {BirdName.Vulture, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Vulture, BirdActivity.Eating},
                                        {BirdName.Peacock, BirdActivity.Ruling},
                                        {BirdName.Cock, BirdActivity.Dying},
                                        {BirdName.Crow, BirdActivity.Walking},
                                        {BirdName.Owl, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Owl, BirdActivity.Eating},
                                        {BirdName.Vulture, BirdActivity.Ruling},
                                        {BirdName.Peacock, BirdActivity.Dying},
                                        {BirdName.Cock, BirdActivity.Walking},
                                        {BirdName.Crow, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Crow, BirdActivity.Eating},
                                        {BirdName.Owl, BirdActivity.Ruling},
                                        {BirdName.Vulture, BirdActivity.Dying},
                                        {BirdName.Peacock, BirdActivity.Walking},
                                        {BirdName.Cock, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Cock, BirdActivity.Eating},
                                        {BirdName.Crow, BirdActivity.Ruling},
                                        {BirdName.Owl, BirdActivity.Dying},
                                        {BirdName.Vulture, BirdActivity.Walking},
                                        {BirdName.Peacock, BirdActivity.Sleeping}
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Friday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Vulture, BirdActivity.Eating},
                                        {BirdName.Peacock, BirdActivity.Ruling},
                                        {BirdName.Cock, BirdActivity.Dying},
                                        {BirdName.Crow, BirdActivity.Walking},
                                        {BirdName.Owl, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Owl, BirdActivity.Eating},
                                        {BirdName.Vulture, BirdActivity.Ruling},
                                        {BirdName.Peacock, BirdActivity.Dying},
                                        {BirdName.Cock, BirdActivity.Walking},
                                        {BirdName.Crow, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Crow, BirdActivity.Eating},
                                        {BirdName.Owl, BirdActivity.Ruling},
                                        {BirdName.Vulture, BirdActivity.Dying},
                                        {BirdName.Peacock, BirdActivity.Walking},
                                        {BirdName.Cock, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Cock, BirdActivity.Eating},
                                        {BirdName.Crow, BirdActivity.Ruling},
                                        {BirdName.Owl, BirdActivity.Dying},
                                        {BirdName.Vulture, BirdActivity.Walking},
                                        {BirdName.Peacock, BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Peacock, BirdActivity.Eating},
                                        {BirdName.Cock, BirdActivity.Ruling},
                                        {BirdName.Crow, BirdActivity.Dying},
                                        {BirdName.Owl, BirdActivity.Walking},
                                        {BirdName.Vulture, BirdActivity.Sleeping}
                                    }
                                }
                            }
                        },
                        {
                            DayOfWeek.Saturday, new Dictionary<int, Dictionary<BirdName, BirdActivity>>
                            {
                                {
                                    1, new Dictionary<BirdName, BirdActivity>
                                    {
                                        {BirdName.Owl,BirdActivity.Eating},
                                        {BirdName.Vulture,BirdActivity.Ruling},
                                        {BirdName.Peacock,BirdActivity.Dying},
                                        {BirdName.Cock,BirdActivity.Walking},
                                        {BirdName.Crow,BirdActivity.Sleeping}
                                    }
                                },
                                {
                                    2, new Dictionary<BirdName, BirdActivity>
                                        {
                                            {BirdName.Crow,BirdActivity.Eating},
                                            {BirdName.Owl,BirdActivity.Ruling},
                                            {BirdName.Vulture,BirdActivity.Dying},
                                            {BirdName.Peacock,BirdActivity.Walking},
                                            {BirdName.Cock,BirdActivity.Sleeping}
                                        }
                                },
                                {
                                    3, new Dictionary<BirdName, BirdActivity>
                                        {
                                            {BirdName.Cock,BirdActivity.Eating},
                                            {BirdName.Crow,BirdActivity.Ruling},
                                            {BirdName.Owl,BirdActivity.Dying},
                                            {BirdName.Vulture,BirdActivity.Walking},
                                            {BirdName.Peacock,BirdActivity.Sleeping}
                                        }
                                },
                                {
                                    4, new Dictionary<BirdName, BirdActivity>
                                        {
                                            {BirdName.Peacock,BirdActivity.Eating},
                                            {BirdName.Cock,BirdActivity.Ruling},
                                            {BirdName.Crow,BirdActivity.Dying},
                                            {BirdName.Owl,BirdActivity.Walking},
                                            {BirdName.Vulture,BirdActivity.Sleeping}
                                        }
                                },
                                {
                                    5, new Dictionary<BirdName, BirdActivity>
                                        {
                                            {BirdName.Vulture,BirdActivity.Eating},
                                            {BirdName.Peacock,BirdActivity.Ruling},
                                            {BirdName.Cock,BirdActivity.Dying},
                                            {BirdName.Crow,BirdActivity.Walking},
                                            {BirdName.Owl,BirdActivity.Sleeping}
                                        }
                                },
                            }
                        }
                    }
                }
            };
    }
}
