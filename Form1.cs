using NAudio.Wave;
using NAudio.Flac;

namespace JankNubTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
        }

        private string introFilePath;
        private string loopFilePath;

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = "Play Once File path: None";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Supported files (*.nub, *.wav, *.mp3, *.snd, *.flac)|*.nub;*.wav;*.mp3;*.snd;*.flac";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                button1_load(filePath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label5.Text = "Play Loop File path: None";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Supported files (*.nub, *.wav, *.mp3, *.snd, *.flac)|*.nub;*.wav;*.mp3;*.snd;*.flac";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                button2_load(filePath);
            }
        }

        private void button1_load(string filePath)
        {
            introFilePath = filePath;
            label4.Text = "Play Once File path:" + Path.GetFullPath(filePath);
        }

        private void button2_load(string filePath)
        {
            loopFilePath = filePath;
            label5.Text = "Play Loop File path:" + Path.GetFullPath(filePath);
        }

        // Merge Generate Button
        private void button3_Click(object sender, EventArgs e)
        {
            if ((label4.Text == "Play Once File path: None") || (label5.Text == "Play Loop File path: None"))
            {
                MessageBox.Show("Conditions to generate the nub file does not met.\nTask aborted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                byte[] headerData = new byte[2048];
                byte[] array1Data = GetArray1Data();

                int playbackRate = int.Parse(comboBox1.SelectedItem.ToString().Replace(" Khz", ""));
                int volumeOffset = 0;
                switch (comboBox2.SelectedItem.ToString())
                {
                    case "Low":
                        volumeOffset = 16128;
                        break;
                    case "Normal":
                        volumeOffset = 16512;
                        break;
                    case "High":
                        volumeOffset = 16576;
                        break;
                    case "Ultra":
                        volumeOffset = 16640;
                        break;
                }

                Array.Copy(array1Data, headerData, array1Data.Length);
                byte[] introPCM = ConvertAndTrimPCM(introFilePath, playbackRate, true, false);
                byte[] loopPCM = ConvertAndTrimPCM(loopFilePath, playbackRate, true, false);

                int loopStartOffset = introPCM.Length; // intro file
                int loopLengthOffset = loopPCM.Length; // loop file
                int mergedOffset = introPCM.Length + loopPCM.Length; // End of merged audio


                // Insert offsets in Big-Endian format
                Array.Copy(IntToBigEndian(mergedOffset), 0, headerData, 20, 4);
                Array.Copy(IntToBigEndian(mergedOffset), 0, headerData, 68, 4);
                Array.Copy(IntToBigEndian(loopStartOffset), 0, headerData, 80, 4);
                Array.Copy(IntToBigEndian(loopLengthOffset - 2048), 0, headerData, 84, 4);
                Array.Copy(IntToBigEndian(volumeOffset), 0, headerData, 102, 2); // thanks nub tools for the hint
                Array.Copy(IntToBigEndian(playbackRate), 0, headerData, 240, 4);

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Save to";
                    saveFileDialog.Filter = "nub file (*.nub)|*.nub";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            fs.Write(headerData, 0, headerData.Length);
                            fs.Write(introPCM, 0, introPCM.Length);
                            fs.Write(loopPCM, 0, loopPCM.Length);
                        }
                        MessageBox.Show("Nub file successfully generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Converts input audio file to PCM 16-bit Big Endian
        private byte[] ConvertAndTrimPCM(string filePath, int playbackRate, bool isGenerate, bool isPreview)
        {
            string ext = Path.GetExtension(filePath).ToLower();

            if (isGenerate || isPreview)
            {
                byte[] rawData = File.ReadAllBytes(filePath);

                // Handle RAW PCM data (assume .nub/.snd are PCM 16-bit stereo)
                if (ext == ".nub" || ext == ".snd")
                {
                    if (isGenerate)
                    {
                        return File.ReadAllBytes(filePath);
                    }

                    if (isPreview)
                    {
                        WaveFormat rawFormat = new WaveFormat(playbackRate, 16, 2);
                        byte[] wavData = ConvertRawToWav(rawData, rawFormat, isBigEndian: true);
                        byte[] trimmedData = TrimSilence(wavData, rawFormat);

                        // Ensure WAV format for preview
                        using (MemoryStream outputMs = new MemoryStream())
                        {
                            using (WaveFileWriter writer = new WaveFileWriter(outputMs, rawFormat))
                            {
                                writer.Write(trimmedData, 0, trimmedData.Length);
                            }
                            return outputMs.ToArray(); // Ensure WAV format
                        }
                    }
                }

                // Convert non-raw audio formats
                using (WaveStream reader = ext == ".flac" ? (WaveStream)new FlacReader(filePath) : new AudioFileReader(filePath))
                {
                    var targetFormat = new WaveFormat(playbackRate, 16, 2);
                    using (var resampler = new MediaFoundationResampler(reader, targetFormat))
                    {
                        resampler.ResamplerQuality = 60;

                        using (MemoryStream outputStream = new MemoryStream())
                        {
                            using (var writer = new WaveFileWriter(outputStream, targetFormat))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = resampler.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    writer.Write(buffer, 0, bytesRead);
                                }
                            }

                            byte[] trimmedData = TrimSilence(outputStream.ToArray(), targetFormat);

                            if (isGenerate)
                            {
                                return ConvertWavToBigEndian(trimmedData);
                            }
                            return trimmedData;
                        }
                    }
                }
            }

            return Array.Empty<byte>();
        }

        private byte[] ConvertWavToBigEndian(byte[] wavData)
        {
            if (wavData.Length < 44) return wavData; // Prevents corruption if data is too short

            byte[] pcmData = (byte[])wavData.Clone();

            for (int i = 44; i < pcmData.Length; i += 2)
            {
                Array.Reverse(pcmData, i, 2); // Swap bytes for each sample
            }

            return pcmData;
        }

        // trim silence for non nub or snd files
        private byte[] TrimSilence(byte[] wavData, WaveFormat format)
        {
            using (var ms = new MemoryStream(wavData))
            using (var reader = new WaveFileReader(ms))
            {
                List<float> samples = new List<float>();
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i += 2)
                    {
                        short sample = BitConverter.ToInt16(buffer, i);
                        float normalized = sample / 32768f; // Normalize to -1.0 to 1.0 range
                        samples.Add(normalized);
                    }
                }

                float silenceThreshold = 0.07f; // it trims away audio that below that threshold

                int startIndex = samples.FindIndex(s => Math.Abs(s) > silenceThreshold);
                if (startIndex == -1) startIndex = 0;

                int endIndex = samples.FindLastIndex(s => Math.Abs(s) > silenceThreshold);
                if (endIndex == -1) endIndex = samples.Count - 1;

                if (startIndex >= endIndex)
                    return Array.Empty<byte>();

                short[] trimmedSamples = samples.Skip(startIndex).Take(endIndex - startIndex + 1)
                                                .Select(f => (short)(f * 32767f))
                                                .ToArray();

                using (var outputMs = new MemoryStream())
                {
                    using (var writer = new WaveFileWriter(outputMs, format))
                    {
                        byte[] sampleBuffer = new byte[trimmedSamples.Length * 2];
                        Buffer.BlockCopy(trimmedSamples, 0, sampleBuffer, 0, sampleBuffer.Length);
                        writer.Write(sampleBuffer, 0, sampleBuffer.Length);
                    }

                    return outputMs.ToArray();
                }
            }
        }

        private byte[] ConvertRawToWav(byte[] rawData, WaveFormat format, bool isBigEndian)
        {
            using (MemoryStream wavStream = new MemoryStream())
            using (WaveFileWriter writer = new WaveFileWriter(wavStream, format))
            {
                if (isBigEndian)
                {
                    for (int i = 0; i < rawData.Length; i += 2)
                    {
                        if (i + 1 < rawData.Length)
                        {
                            // Swap bytes (Big-Endian ¡ú Little-Endian)
                            (rawData[i], rawData[i + 1]) = (rawData[i + 1], rawData[i]);
                        }
                    }
                }

                // Write converted PCM data to WAV file
                writer.Write(rawData, 0, rawData.Length);
                writer.Flush();
                return wavStream.ToArray();
            }
        }

        // Converts an integer to a 4-byte Big-Endian array
        private byte[] IntToBigEndian(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }

        // Generate loop button
        private void button4_Click(object sender, EventArgs e)
        {
            if (!(label4.Text == "Play Once File path: None") || (label5.Text == "Play Loop File path: None"))
            {
                MessageBox.Show("Conditions to generate the nub file does not met.\nTask aborted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                byte[] headerData = new byte[2048];
                byte[] array1Data = GetArray1Data();
                Array.Copy(array1Data, headerData, array1Data.Length);

                int playbackRate = int.Parse(comboBox1.SelectedItem.ToString().Replace(" Khz", ""));
                int volumeOffset = 0;
                switch (comboBox2.SelectedItem.ToString())
                {
                    case "Low":
                        volumeOffset = 16128;
                        break;
                    case "Normal":
                        volumeOffset = 16512;
                        break;
                    case "High":
                        volumeOffset = 16576;
                        break;
                    case "Ultra":
                        volumeOffset = 16640;
                        break;
                }

                byte[] loopPCM = ConvertAndTrimPCM(loopFilePath, playbackRate, true, false);

                int loopLengthOffset = loopPCM.Length; // loop file

                Array.Copy(IntToBigEndian(loopLengthOffset), 0, headerData, 20, 4);
                Array.Copy(IntToBigEndian(loopLengthOffset), 0, headerData, 68, 4);
                Array.Copy(IntToBigEndian(loopLengthOffset - 2048), 0, headerData, 84, 4);
                Array.Copy(IntToBigEndian(volumeOffset), 0, headerData, 102, 2); // thanks nub tools for the hint
                Array.Copy(IntToBigEndian(playbackRate), 0, headerData, 240, 4);

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Save to";
                    saveFileDialog.Filter = "nub file (*.nub)|*.nub";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            fs.Write(headerData, 0, headerData.Length);
                            fs.Write(loopPCM, 0, loopPCM.Length);
                        }
                        MessageBox.Show("Nub file successfully generated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] GetArray1Data()
        {
            return new byte[]
            {
                0x01,0x02,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0x00,0x00,0x00,
                0x00,0x08,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x20,0x00,0x00,0x00,0x30,0x00,0x00,0x00,
                0x30,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0x77,0x61,0x76,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x00,0x00,0x10,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0xFF,0xFF,0xFF,0xFF,0x00,0x00,0x80,0x40,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x70,0x42,0x00,0x00,0x80,0x3F,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x80,0x3F,0x00,0x00,0x80,0x3F,
                0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xC8,0xC2,
                0xE8,0x03,0x00,0x00,0x64,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x80,0x3F,
                0x14,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x80,0x3F,0x00,0x00,0x00,0x00,
                0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,0x00,0x02,0x00,
                0x44,0xAC,0x00,0x00,0x10,0xB1,0x02,0x00,0x04,0x00,0x10,0x00,0x00,0x00,0x00,0x00
            };
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int playbackRate = int.Parse(comboBox1.SelectedItem.ToString().Replace(" Khz", ""));

                byte[] audioIntro = null;
                byte[] audioLoop = ConvertAndTrimPCM(loopFilePath, playbackRate, false, true);

                if (!string.IsNullOrEmpty(introFilePath))
                {
                    audioIntro = ConvertAndTrimPCM(introFilePath, playbackRate, false, true);
                }

                if ((audioIntro == null || audioIntro.Length == 0) && (audioLoop == null || audioLoop.Length == 0))
                {
                    MessageBox.Show("Failed to generate audio preview. Both audio files are empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (audioLoop == null || audioLoop.Length == 0)
                {
                    MessageBox.Show("Failed to generate loop track preview. The file may be unsupported or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                byte[] audioData;
                if (audioIntro == null || audioIntro.Length == 0)
                {
                    // Skip intro if not selected
                    audioData = audioLoop;
                }
                else
                {
                    // Merge intro and loop
                    audioData = new byte[audioIntro.Length + audioLoop.Length];
                    Buffer.BlockCopy(audioIntro, 0, audioData, 0, audioIntro.Length);
                    Buffer.BlockCopy(audioLoop, 0, audioData, audioIntro.Length, audioLoop.Length);
                }

                // Open PlaybackForm with merged audio
                PlaybackForm preview = new PlaybackForm(audioIntro, audioLoop, audioData, playbackRate, 16, 2);
                preview.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "nub files (*.nub)|*.nub"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        fs.Seek(240, SeekOrigin.Begin);
                        // Read playback rate (big-endian)
                        int playbackRate = reader.ReadInt32();

                        // Create wave format
                        WaveFormat rawFormat = new WaveFormat(playbackRate, 16, 2);

                        // Read intro and loop offsets (big-endian)
                        fs.Seek(80, SeekOrigin.Begin);
                        int loopStart = reader.ReadInt32();

                        fs.Seek(84, SeekOrigin.Begin);
                        int loopLength = reader.ReadInt32();

                        byte[] introRawData = reader.ReadBytes(loopStart);
                        byte[] loopRawData = reader.ReadBytes(loopLength);

                        // Convert raw PCM to WAV
                        byte[] audioIntro = ConvertRawToWav(introRawData, rawFormat, isBigEndian: true);
                        byte[] audioLoop = ConvertRawToWav(loopRawData, rawFormat, isBigEndian: true);

                        // Merge for preview
                        byte[] fullAudio = new byte[audioIntro.Length + audioLoop.Length];
                        Buffer.BlockCopy(audioIntro, 0, fullAudio, 0, audioIntro.Length);
                        Buffer.BlockCopy(audioLoop, 0, fullAudio, audioIntro.Length, audioLoop.Length);

                        // Open PlaybackForm
                        PlaybackForm preview = new PlaybackForm(audioIntro, audioLoop, fullAudio, playbackRate, 16, 2);
                        preview.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            VolumeChangeForm volumeChange = new VolumeChangeForm();
            volumeChange.Show();
        }
    }

    public partial class PlaybackForm : Form
    {
        private IWavePlayer waveOut;
        private WaveStream waveStream;
        private WaveStream waveLoopStream;
        private byte[] audioData;
        private byte[] introAudioData;
        private byte[] loopAudioData;
        private WaveFormat waveFormat;
        private bool isLooping;

        public PlaybackForm(byte[] introAudioData, byte[] loopAudioData, byte[] audioData, int sampleRate, int bitDepth, int channels)
        {
            InitializePlay();

            this.introAudioData = introAudioData;
            this.loopAudioData = loopAudioData;
            this.audioData = audioData;
            this.waveFormat = new WaveFormat(sampleRate, bitDepth, channels);

            this.FormClosed += PlaybackForm_FormClosed;
        }

        private void PlaybackForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopAudio();
        }

        private void PlayOnceButton_Click(object sender, EventArgs e)
        {
            PlayAudio(false);
            playOnceButton.Text = "Playing";
        }

        private void PlayLoopButton_Click(object sender, EventArgs e)
        {
            PlayAudio(true);
            playLoopButton.Text = "Playing";
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StopAudio();
        }

        private void PlayAudio(bool loop)
        {
            StopAudio(); // Stop any existing playback

            isLooping = loop; // Track loop state

            if (loop)
            {
                if (introAudioData != null && introAudioData.Length > 0)
                {
                    MemoryStream msIntro = new MemoryStream(introAudioData);
                    MemoryStream msLoop = new MemoryStream(loopAudioData);

                    // Use custom IntroLoopStream to handle intro-to-loop transition seamlessly
                    waveStream = new IntroLoopStream(new RawSourceWaveStream(msIntro, waveFormat),
                                                     new RawSourceWaveStream(msLoop, waveFormat));

                    waveOut = new WaveOutEvent();
                    waveOut.Init(waveStream);
                    waveOut.Play();
                }
                else
                {
                    // If no intro exists, just loop the main audio
                    PlayLoopAudio();
                    return;
                }
            }
            else
            {
                // Play audio once and stop
                MemoryStream ms = new MemoryStream(audioData);
                waveStream = new RawSourceWaveStream(ms, waveFormat);

                waveOut = new WaveOutEvent();
                waveOut.Init(waveStream);

                waveOut.PlaybackStopped += (sender, args) => StopAudio();

                waveOut.Play();
            }
        }


        private void PlayLoopAudio()
        {
            MemoryStream msLoop = new MemoryStream(loopAudioData);
            waveLoopStream = new RawSourceWaveStream(msLoop, waveFormat);
            waveStream = new LoopStream(waveLoopStream); // Wrap with looping behavior

            waveOut = new WaveOutEvent();
            waveOut.Init(waveStream);
            waveOut.Play();
        }

        private void StopAudio()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
            waveOut = null;

            waveStream?.Dispose();
            waveStream = null;

            if (playOnceButton.Text == "Playing")
            {
                playOnceButton.Text = "Play Once";
            }
            if (playLoopButton.Text == "Playing")
            {
                playLoopButton.Text = "Play Loop";
            }
        }
    }

    // LoopStream class to handle looping
    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;

        public override long Length => sourceStream.Length;

        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    sourceStream.Position = 0; // Restart the stream
                }
                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }
    }

    public class IntroLoopStream : WaveStream
    {
        private readonly WaveStream introStream;
        private readonly WaveStream loopStream;
        private bool introFinished = false;

        public IntroLoopStream(WaveStream intro, WaveStream loop)
        {
            introStream = intro;
            loopStream = loop;
        }

        public override WaveFormat WaveFormat => introStream.WaveFormat;

        public override long Length => long.MaxValue; // Infinite length for looping

        public override long Position
        {
            get => introFinished ? loopStream.Position + introStream.Length : introStream.Position;
            set
            {
                if (value < introStream.Length)
                {
                    introStream.Position = value;
                    introFinished = false;
                }
                else
                {
                    long loopPos = (value - introStream.Length);
                    loopStream.Position = Math.Min(loopPos, loopStream.Length - 1); // Ensure we don¡¯t cut early
                    introFinished = true;
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = 0;

            if (!introFinished)
            {
                bytesRead = introStream.Read(buffer, offset, count);
                if (bytesRead < count)
                {
                    introFinished = true;
                }
            }

            while (bytesRead < count)
            {
                int loopBytesRead = loopStream.Read(buffer, offset + bytesRead, count - bytesRead);

                if (loopBytesRead == 0)
                {
                    // Instead of restarting immediately, ensure full loop plays before reset
                    loopStream.Position = 0;
                    loopBytesRead = loopStream.Read(buffer, offset + bytesRead, count - bytesRead);
                }

                bytesRead += loopBytesRead;
            }

            return bytesRead;
        }
    }

    public partial class VolumeChangeForm : Form
    {
        public VolumeChangeForm()
        {
            InitializeVolumeChange();
            dataGridView1.DragEnter += dataGridView1_DragEnter;
            dataGridView1.DragDrop += dataGridView1_DragDrop;
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the data being dragged is a file
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; // Allow copying the file
            }
            else
            {
                e.Effect = DragDropEffects.None; // Do not allow other types
            }
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                // Retrieve the file paths
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Loop for multiple file dropped in datagridview
                if (filePaths != null)
                {
                    dataGridView1.Rows.Clear();

                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        // Validate the file name
                        if (Path.GetFileName(filePaths[i]).EndsWith(".nub"))
                        {
                            LoadTrackData(filePaths[i]);
                        }
                        else
                        {
                            MessageBox.Show(
                                "The dropped file is not a valid nub file.",
                                "Invalid File",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while processing the dropped file: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get selected volume level
            string selectedVolume = comboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedVolume))
            {
                MessageBox.Show("Please select a volume level before applying.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int updatedCount = 0;
            int skippedCount = 0;

            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue; // Skip placeholder row if any

                string filePath = row.Cells["File Path"].Value?.ToString();
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) continue;

                byte[] fileBytes = File.ReadAllBytes(filePath);
                int volumeOffset = selectedVolume switch
                {
                    "Low" => 16128,
                    "Normal" => 16512,
                    "High" => 16576,
                    "Ultra" => 16640
                };

                byte[] volumeBytes = BitConverter.GetBytes((ushort)volumeOffset);

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.Seek(0x66, SeekOrigin.Begin);
                    byte[] currentVolumeBytes = new byte[2];
                    fs.Read(currentVolumeBytes, 0, 2);

                    ushort currentVolume = BitConverter.ToUInt16(currentVolumeBytes, 0);
                    if (currentVolume == (ushort)volumeOffset)
                    {
                        skippedCount++;
                        continue;
                    }

                    fs.Seek(0x66, SeekOrigin.Begin);
                    fs.Write(volumeBytes, 0, volumeBytes.Length);
                    updatedCount++;
                }

                // Optionally update the grid view
                row.Cells["Detected Volume"].Value = selectedVolume;
            }

            MessageBox.Show(
                $"Volume successfully applied.\n\n" +
                $"Updated: {updatedCount} file(s)\n" +
                $"Skipped: {skippedCount} file(s already at selected volume)",
                "Result",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void LoadTrackData(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fs.Seek(0x66, SeekOrigin.Begin);
                    int volume = reader.ReadInt16();

                    string volumeLabel = "";
                    switch(volume)
                    {
                        case 16128:
                            volumeLabel = "Low";
                            break;
                        case 16512:
                            volumeLabel = "Normal";
                            break;
                        case 16576:
                            volumeLabel = "High";
                            break;
                        case 16640:
                            volumeLabel = "Ultra";
                            break;
                        default:
                            volumeLabel = "Unknown Volume";
                            break;
                    }

                    dataGridView1.Rows.Add(
                        $"{filePath}",
                        $"{volumeLabel}"
                        );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the file:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
