// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: AudioEqualizer.cs
// Version: 20181231

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Meta.Vlc.Interop.MediaPlayer.Audio;

namespace Meta.Vlc
{
    /// <summary>
    ///     Audio equalizer of VLC player.
    /// </summary>
    public unsafe class AudioEqualizer : IUnmanagedObject, IEnumerable<float>, INotifyPropertyChanged
    {
        #region --- Fields ---

        private bool _disposed;

        #endregion --- Fields ---

        private class AudioEqualizerEnumerator : IEnumerator<float>
        {
            private readonly AudioEqualizer _audioEqualizer;
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

            public float Current => _audioEqualizer[(uint) _index];

            object IEnumerator.Current => Current;
        }

        #region --- Initialization ---

        /// <summary>
        ///     Create a new default equalizer, with all frequency values zeroed.
        /// </summary>
        public AudioEqualizer()
        {
            InstancePointer = LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_new>().Invoke();
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
            InstancePointer = LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_new_from_preset>().Invoke(index);
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_release>().Invoke(InstancePointer);
            InstancePointer = null;

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
        public static uint PresetEqualizerCount =>
            LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_get_preset_count>().Invoke();

        /// <summary>
        ///     Get the number of distinct frequency bands for an equalizer.
        /// </summary>
        public static uint EqualizerBandCount =>
            LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_get_band_count>().Invoke();

        public void* InstancePointer { get; private set; }

        /// <summary>
        ///     Get or set the current pre-amplification value from an equalizer.
        /// </summary>
        public float Preamp
        {
            get => LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_get_preamp>().Invoke(InstancePointer);
            set
            {
                OnPropertyChanged(nameof(Preamp));
                LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_set_preamp>().Invoke(InstancePointer, value);
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
                    throw new IndexOutOfRangeException("Band index should less than AudioEqualizer.EqualizerBandCount");

                return LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_get_amp_at_index>()
                    .Invoke(InstancePointer, band);
            }
            set
            {
                if (band >= EqualizerBandCount)
                    throw new IndexOutOfRangeException("Band index should less than AudioEqualizer.EqualizerBandCount");

                OnPropertyChanged(null);
                LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_set_amp_at_index>()
                    .Invoke(InstancePointer, value, band);
            }
        }

        #endregion --- Properties ---

        #region --- Methods ---

        /// <summary>
        ///     Get the name of a particular equalizer preset.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetPresetEqualizerName(uint index)
        {
            return InteropHelper.PtrToString(LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_get_preset_name>()
                .Invoke(index));
        }

        /// <summary>
        ///     Get a particular equalizer band frequency.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static float GetEqualizerBandFrequency(uint index)
        {
            return LibVlcManager.GetFunctionDelegate<libvlc_audio_equalizer_get_band_frequency>().Invoke(index);
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion --- NotifyPropertyChanged ---

        #endregion --- Methods ---
    }
}