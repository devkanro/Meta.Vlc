// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: EndBehavior.cs
// Version: 20160214

namespace Meta.Vlc.Wpf
{
    /// <summary>
    ///     Define the behavior when media is ended.
    /// </summary>
    public enum EndBehavior
    {
        /// <summary>
        ///     Do nothing, player's state is Ended, you need stop the player to play current media again.
        /// </summary>
        Nothing,

        /// <summary>
        ///     Stop the player.
        /// </summary>
        Stop,

        /// <summary>
        ///     Play current media again.
        /// </summary>
        Repeat,

        /// <summary>
        ///     Default behavior, same as Stop.
        /// </summary>
        Default = Stop
    }
}