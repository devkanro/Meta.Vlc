using System;
using xZune.Vlc.Interop;
using xZune.Vlc.Interop.MediaPlayer;

namespace xZune.Vlc
{
    /// <summary>
    /// Audio equalizer of VLC.
    /// </summary>
    public class AudioEqualizer : IVlcObject
    {
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
        /// Create a new default equalizer, with all frequency values zeroed.
        /// </summary>
        public AudioEqualizer()
        {
            InstancePointer = _createEqualizerFunction.Delegate();
            HandleManager.Add(this);
        }

        /// <summary>
        /// Create a new equalizer, with initial frequency values copied from an existing preset.
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
        /// 获取一个值,该值指示当前模块是否被载入
        /// </summary>
        public static bool IsLibLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the number of equalizer presets.
        /// </summary>
        public static uint PresetEqualizerCount
        {
            get { return _getEqualizerPresetCountFunction.Delegate(); }
        }

        /// <summary>
        /// Get the number of distinct frequency bands for an equalizer.
        /// </summary>
        public static uint EqualizerBandCount
        {
            get { return _getEqualizerBandCountFunction.Delegate(); }
        }

        public IntPtr InstancePointer { get; private set; }

        /// <summary>
        /// Get or set the current pre-amplification value from an equalizer.
        /// </summary>
        public float Preamp
        {
            get { return _getEqualizerPreampFunction.Delegate(InstancePointer); }
            set { _setEqualizerPreampFunction.Delegate(InstancePointer, value); }
        }

        /// <summary>
        /// Get or set the amplification value for a particular equalizer frequency band. 
        /// </summary>
        /// <param name="band">frequency band index.</param>
        /// <returns></returns>
        public float this[uint band] 
        {
            get { return _getEqualizerAmplificationFunction.Delegate(InstancePointer, band); }
            set { _setEqualizerAmplificationFunction.Delegate(InstancePointer, value, band); }
        }

        #endregion

        #region --- Methods ---

        /// <summary>
        /// 载入 LibVlc 的 AudioEqualizer 模块,该方法会在 <see cref="Vlc.LoadLibVlc()"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        /// <param name="devString"></param>
        public static void LoadLibVlc(IntPtr libHandle, Version libVersion, String devString)
        {
            if(IsLibLoaded) return;
            
            _createEqualizerFunction = new LibVlcFunction<CreateEqualizer>(libHandle, libVersion, devString);
            _createEqualizerFromPresetFunction = new LibVlcFunction<CreateEqualizerFromPreset>(libHandle, libVersion, devString);
            _releaseEqualizerFunction = new LibVlcFunction<ReleaseEqualizer>(libHandle, libVersion, devString);
            _getEqualizerPresetCountFunction = new LibVlcFunction<GetEqualizerPresetCount>(libHandle, libVersion, devString);
            _getEqualizerPresetNameFunction = new LibVlcFunction<GetEqualizerPresetName>(libHandle, libVersion, devString);
            _getEqualizerBandCountFunction = new LibVlcFunction<GetEqualizerBandCount>(libHandle, libVersion, devString);
            _getEqualizerBandFrequencyFunction = new LibVlcFunction<GetEqualizerBandFrequency>(libHandle, libVersion, devString);
            _setEqualizerPreampFunction = new LibVlcFunction<SetEqualizerPreamp>(libHandle, libVersion, devString);
            _getEqualizerPreampFunction = new LibVlcFunction<GetEqualizerPreamp>(libHandle, libVersion, devString);
            _setEqualizerAmplificationFunction = new LibVlcFunction<SetEqualizerAmplification>(libHandle, libVersion, devString);
            _getEqualizerAmplificationFunction = new LibVlcFunction<GetEqualizerAmplification>(libHandle, libVersion, devString);

            IsLibLoaded = true;
        }

        /// <summary>
        /// Get the name of a particular equalizer preset.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static String GetPresetEqualizerName(uint index)
        {
            return InteropHelper.PtrToString(_getEqualizerPresetNameFunction.Delegate(index),-1,true);
        }

        /// <summary>
        /// Get a particular equalizer band frequency.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static float GetEqualizerBandFrequency(uint index)
        {
            return _getEqualizerBandFrequencyFunction.Delegate(index);
        }
        #endregion
    }
}