# OpenDACT (Open Delta Automatic Calibration Tool)

OpenDACT is an open-source application for automatically calibrating delta-style 3D printers. It is written in C# and features a graphical user interface (GUI) that sends G-code commands directly to your printer—either through Repetier or manual setup. This tool aims to simplify and automate the often time-consuming process of calibrating delta kinematics.

> **DISCLAIMER**  
> This software is provided as-is, with no guarantee that it will function correctly in every situation. Always monitor your printer when using OpenDACT or any other software that sends commands to your machine. Neither the developers nor contributors assume responsibility for damage or loss caused by the use of this software.

---

## Features

- **Automatic Calibration:** Uses probing points and real-time calculations to adjust tower angles, endstops, delta radius, and other critical parameters of your delta printer.
- **Graphical Interface:** Simple GUI to guide users through configuration, calibration, and manual adjustments without the need to edit firmware files by hand.
- **Manual Control (Optional):** Offers a manual mode for advanced users who want to control calibration steps on their own.
- **G-code Execution:** Sends G-code commands directly to the printer via serial communication (e.g., Repetier or any supported USB interface).
- **Customizable Settings:** Users can change calibration parameters, set probe points, or configure advanced options for different firmware types.

---

## Getting Started

### Prerequisites

- A **Windows** environment (OpenDACT is a C# application; it may work on other platforms via [Mono](https://www.mono-project.com/), but is primarily tested on Windows).
- A **delta-style 3D printer** (RepRap compatible) running firmware (e.g., Repetier, or others that accept standard G-code).
- A **USB connection** to your printer (or another supported serial interface).
- The **.NET Framework** (typically ships with Windows or can be installed from [Microsoft](https://dotnet.microsoft.com/)).

### Installation

1. **Download the latest release** and compile the source from this repository.
2. **Build** the program.
3. Run the **OpenDACT.exe** file or run from Visual Studio.

### First Launch & Setup

1. **Connect your Printer:**
   - Plug in your 3D printer via USB.
   - Select the correct COM port and baud rate in OpenDACT’s Settings.

2. **Initial Configuration:**
   - Enter your printer’s build dimensions (height, radius).
   - Optionally configure any custom firmware commands or offsets if your setup is non-standard.

3. **Probe Setup:**
   - If you have a probe or endstop adjustments, configure them here.
   - Set the probe trigger offset if required by your hardware.

4. **Calibration Process:**
   - Click the **Calibrate** button to start the automatic calibration sequence.
   - Monitor the printer as it probes different points on the build surface to measure deviations.
   - OpenDACT will adjust firmware parameters (endstop offsets, delta radius, tower angles, etc.) based on the measurements.

---

## Usage Tips

1. **Stay Near the Printer:**  
   Calibration involves moving the print head around, sometimes very close to the build surface. Always watch for mechanical issues or collisions.

2. **Backup Your Firmware Settings:**  
   Before using OpenDACT, backup your existing firmware configuration (e.g., using M503 in Repetier/Marlin to save or record your settings).

3. **Small Steps, Frequent Tests:**  
   If you’re unsure about the perfect calibration routine, run small sets of calibration cycles and test print after each round. Over-calibration or incorrect inputs can lead to print errors.

4. **Update Firmware Parameters Manually (If Needed):**  
   Should OpenDACT fail to update parameters, or if your firmware isn't fully supported, you can still read the recommended adjustments from OpenDACT and enter them into your printer’s configuration manually.

---

## Contributing

We welcome contributions from the community! If you would like to help improve OpenDACT, you can:

- **Submit Issues:** Found a bug or have a feature request? [Open an issue](#) on our GitHub.
- **Create Pull Requests:** Fork this repository, commit your changes, and submit a PR.
- **Contact the Developer:**  
  - Email: [coelacannot (at) gmail.com](mailto:coelacannot@gmail.com)  
  - Or open an issue on Github.

Please note: We recommend you discuss your ideas with us before implementing large changes to ensure alignment and avoid duplicates.

---

## Roadmap

- Improved firmware compatibility for more advanced delta printers
- Expanded user interface for real-time calibration feedback
- Enhanced manual modes for expert users
- Additional 3D visualization of calibration points

---

## License

OpenDACT is licensed under the [AGPL-3.0](./LICENSE). You are free to modify and distribute the code, but please give credit to the original authors and contributors.

---

## Forum Post

http://forum.seemecnc.com/viewtopic.php?f=36&t=8698
