# Text File Generator and Sorter

This repository contains C# program that has 2 capabilities:
1. **File Generator**: Generates a test file of a specified size containing lines of the form `Number. String`.
2. **File Sorter**: Sorts large text files based on the string component first and the number second. Designed to handle very large files efficiently.


![Generated output](https://github.com/BigYellowHammer/Sorter/blob/main/samples/image.png?raw=true)
---

## Features

### File Generator
- Generates a text file with lines formatted as `Number. String`.
- Allows the specification of file size.

### File Sorter
- Handles sorting of very large files using external merge sort to manage memory constraints.
- Sorts lines primarily by the string part and secondarily by the numeric part.
- Outputs the sorted file to a specified location.

---

## Getting Started

### Prerequisites
- **.NET 8.0 SDK**
- A development environment such as Visual Studio, Visual Studio Code, or JetBrains Rider.

### Installation
Clone this repository to your local machine:
```bash
git clone https://github.com/BigYellowHammer/Sorter.git
cd Sorter
dotnet build --configuration Release
```

---

## Usage

### 1. File Generator
The **File Generator** creates a test file of a specified size.

#### Command-Line Arguments:
- **File Size (in bytes)**: The approximate size of the file to generate.
- **-i ../file/path/file.txt**: **[OPTIONAL]** Input dictionary used for string generation
- **-o ../file/path/file.txt**: **[OPTIONAL]** Output file location (default output.txt)
- **-progress**: **[OPTIONAL]** Shows generation progress -> not recommended for large generations as its slows down execution

#### Example:
```bash
./GenSort.exe generate 1024 -i ../samples/words.txt -o customOutputFile.txt --progress
```

This generates a `customOutputFile.txt` file of size 1024 bytes and uses `words.txt` dictionary to generate it. Progress percentage is presented during generation.

---

### 2. File Sorter
The **File Sorter** reads a large text file and sorts it based on the string part first and the numeric part second.

#### Command-Line Arguments:
- **Input File Path**: Path of the file to be sorted.
- **-o customSortedFile.txt**: **[OPTIONAL]** Path to save the sorted file. (default sorted.txt)
- **-progress**: **[OPTIONAL]** Shows sorting progress

#### Example:
```bash
./GenSort.exe sort output.txt -o customSortedFile.txt --progress
```

This processes `output.txt` and saves the sorted result in `customSortedFile.txt`. Progress percentage is presented during sorting.

---

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## Contributions
Contributions are welcome! Feel free to submit issues or pull requests.

---