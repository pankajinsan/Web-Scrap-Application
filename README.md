# Web Scrap Application

A .NET C# console application designed for web scraping lottery winning numbers and results data (such as Texas Lottery Lotto Texas) using HtmlAgilityPack and formatting output data.

## Overview

`WebScrapApplication` fetches winning numbers, drawing dates, prize amounts, and city location data from lottery web pages. It parses HTML DOM elements using HTML Agility Pack, formats the extracted fields into pipe-separated structured strings, and writes the output dataset to a text file.

## Features

- **Automated Web Scraping**: Fetches dynamic web pages and scrapes lottery detail links and draw data.
- **HTML Parsing**: Uses `HtmlAgilityPack` to query DOM nodes (`h3`, `ol.winningNumberBalls`, `table` elements) using XPath queries.
- **Data Formatting**: Formats winning numbers, drawing dates, prize values, and winning location cities into standardized pipe-delimited records.
- **Secure Network Calls**: Configures modern TLS protocols (`SecurityProtocolType.Tls12`) for reliable HTTPS communication.

## Technologies & Dependencies

- **Framework**: .NET Framework / C#
- **HtmlAgilityPack**: HTML parsing and XPath selection.
- **CsvHelper**: Library for reading and writing CSV/formatted data.
- **ObjectDumper**: Utility for dumping object contents during debugging/logging.

## Project Structure

```text
WebScrapApplication/
├── Program.cs                # Main application entry point and scraping logic
├── Cash5.cs                  # Model class for lottery record representations
├── App.config                # Application configuration settings
├── packages.config           # NuGet package dependencies configuration
└── WebScrapApplication.csproj# Visual Studio C# Project configuration
```

## Getting Started & Usage

### Prerequisites

- [.NET SDK / Visual Studio](https://dotnet.microsoft.com/)
- NuGet package manager

### Building & Running

1. Clone or open the repository in Visual Studio / MSBuild environment:
   ```bash
   git clone https://github.com/pankajinsan/Web-Scrap-Application.git
   ```
2. Restore NuGet dependencies:
   ```bash
   nuget restore WebScrapApplication.sln
   ```
3. Build and execute the console application:
   ```bash
   dotnet run --project WebScrapApplication/WebScrapApplication.csproj
   ```

The application will process lottery winning draw details and output formatted results to the specified output file location.
