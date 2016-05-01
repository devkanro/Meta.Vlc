// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AudioEqualizer.cs
// Version: 20160214

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Meta.Vlc.Interop;
using Meta.Vlc.Interop.MediaPlayer;

namespace Meta.Vlc
{
    /// <summary>
    ///     Audio equalizer of VLC player.
    /// </summary>
    public class AudioEqualizer : IVlcObject, IEnumerable<float>, INotifyPropertyChanged
    {
        private class AudioEqualizerEnumerator : IEnumerator<float>
        {
            private AudioEqualizer _audioEqualizer;
            private int _index = -1;

            public AudioEqualizerEnumerator(AudioEqualizer equalizer)
            {
                _audioEqualizer = equalizer;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < EqualizerBandCount - 1)
                {
                    _index++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = -1;
            }

            public float Current
            {
                get { return _audioEqualizer[(uint) _index]; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        #region --- Fields ---

        private bool _disposed;

        #region LibVlcFunctions

        private static LibVlcFunction<CreateEqualizer> _createEqualizerFunction;
        private static LibVlcFunction<CreateEqualizerFromPreset> _createEqualizerFromPresetFunction;
        private static LibVlcFunction<ReleaseEqualizer> _releaseEqualizerFunction;
        private static LibVlcFunction<GetEqualizerPresetCount> _getEqualizerPresetCountFunction;
        private static LibVlcFunction<GetEqualizerPresetName> _getEqualizerPresetNameFunction;
        private static LibVlcFunction<GetEqualizerBandCount> _getEqualizerBandCountFunction;
        private static LibVlcFunction<GetEqualizerBandFrequency> _getEqualizerBandFrequencyFunction;
        private static LibVlcFunction<SetEqualizerPreamp> _setEqualizerPreampFunction;
        private static LibVlcFunction<GetEqualizerPreamp> _getEqualizerPreampFunction;
        private static LibVlcFunction<SetEqualizerAmplification> _setEqualizerAmplificationFunction;
        private static LibVlcFunction<GetEqualizerAmplification> _getEqualizerAmplificationFunction;

        #endregion LibVlcFunctions

        #endregion --- Fields ---

        #region --- Initialization ---

        /// <summary>
        ///     Create a new default equalizer, with all frequency values zeroed.
        /// </summary>
        public AudioEqualizer()
        {
            InstancePointer = _createEqualizerFunction.Delegate();
            HandleManager.Add(this);
        }

        /// <summary>
        ///     Create a new equalizer, with initial frequency values copied from an existing preset.
        /// </summary>
        /// <param name="type"></param>
        public AudioEqualizer(PresetAudioEqualizerType type) : this((uint) type)
        {
        }

        /// <summary>
        ///     Create a new equalizer, with initial frequency values copied from an existing preset.
        /// </summary>
        /// <param name="index"></param>
        public AudioEqualizer(uint index)
        {
            InstancePointer = _createEqualizerFromPresetFunction.Delegate(index);
            HandleManager.Add(this);
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            HandleManager.Remove(this);

            _releaseEqualizerFunction.Delegate(InstancePointer);

            InstancePointer = IntPtr.Zero;

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion --- Cleanup ---

        #region --- Properties ---

        /// <summary>
        ///     获取一个值,该值指示当前模块是否被载入
        /// </summary>
        public static bool IsLibLoaded { get; private set; }

        /// <summary>
        ///     Get the number of equalizer presets.
        /// </summary>
        public static uint PresetEqualizerCount
        {
            get { return _getEqualizerPresetCountFunction.Delegate(); }
        }

        /// <summary>
        ///     Get the number of distinct frequency bands for an equalizer.
        /// </summary>
        public static uint EqualizerBandCount
        {
            get { return _getEqualizerBandCountFunction.Delegate(); }
        }

        public IntPtr InstancePointer { get; private set; }

        /// <summary>
        ///     Aways return <see cref="null" />.
        /// </summary>
        public Vlc VlcInstance
        {
            get { return null; }
        }

        /// <summary>
        ///     Get or set the current pre-amplification value from an equalizer.
        /// </summary>
        public float Preamp
        {
            get { return _getEqualizerPreampFunction.Delegate(InstancePointer); }
            set
            {
                OnPropertyChanged("Preamp");
                _setEqualizerPreampFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        ///     Get or set the amplification value for a particular equalizer frequency band.
        /// </summary>
        /// <param name="band">frequency band index.</param>
        /// <returns></returns>
        public float this[uint band]
        {
            get
            {
                if (band >= EqualizerBandCount)
                {
                    throw new IndexOutOfRangeException("Band index should less than AudioEqualizer.EqualizerBandCount");
                }
                return _getEqualizerAmplificationFunction.Delegate(InstancePointer, band);
            }
            set
            {
                if (band >= EqualizerBandCount)
                {
                    throw new IndexOutOfRangeException("Band index should less than AudioEqualizer.EqualizerBandCount");
                }

                OnPropertyChanged(null);
                _setEqualizerAmplificationFunction.Delegate(InstancePointer, value, band);
            }
        }

        #endregion --- Properties ---

        #region --- Methods ---

        internal static void LoadLibVlc()
        {
            if (IsLibLoaded) return;

            _createEqualizerFunction = new LibVlcFunction<CreateEqualizer>();
            _createEqualizerFromPresetFunction = new LibVlcFunction<CreateEqualizerFromPreset>();
            _releaseEqualizerFunction = new LibVlcFunction<ReleaseEqualizer>();
            _getEqualizerPresetCountFunction = new LibVlcFunction<GetEqualizerPresetCount>();
            _getEqualizerPresetNameFunction = new LibVlcFunction<GetEqualizerPresetName>();
            _getEqualizerBandCountFunction = new LibVlcFunction<GetEqualizerBandCount>();
            _getEqualizerBandFrequencyFunction = new LibVlcFunction<GetEqualizerBandFrequency>();
            _setEqualizerPreampFunction = new LibVlcFunction<SetEqualizerPreamp>();
            _getEqualizerPreampFunction = new LibVlcFunction<GetEqualizerPreamp>();
            _setEqualizerAmplificationFunction = new LibVlcFunction<SetEqualizerAmplification>();
            _getEqualizerAmplificationFunction = new LibVlcFunction<GetEqualizerAmplification>();

            IsLibLoaded = true;
        }

        /// <summary>
        ///     Get the name of a particular equalizer preset.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static String GetPresetEqualizerName(uint index)
        {
            return InteropHelper.PtrToString(_getEqualizerPresetNameFunction.Delegate(index));
        }

        /// <summary>
        ///     Get a particular equalizer band frequency.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static float GetEqualizerBandFrequency(uint index)
        {
            return _getEqualizerBandFrequencyFunction.Delegate(index);
        }

        public IEnumerator<float> GetEnumerator()
        {
            return new AudioEqualizerEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region --- NotifyPropertyChanged ---

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion --- NotifyPropertyChanged ---

        #endregion --- Methods ---
    }
}