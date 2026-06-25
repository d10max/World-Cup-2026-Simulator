# World Cup 2026 Simulator

A C# console application that simulates the 2026 FIFA World Cup. It supports the expanded 48-team format and provides both a single-tournament simulation with console output and a statistical analysis mode.

## Key Features

* **2026 Format Compliance:** Implements the 48-team group stage and the algorithm for advancing the 8 best third-placed teams into the Round of 32 knockout bracket.
* **Match Simulation Engine:** Calculates goals using Inverse Transform Sampling based on the Poisson distribution.
* **Power Index Calculation:** Determines team strength by applying a logarithmic function to squad market values, followed by Min-Max normalization (scaling to 1-100).
* **Execution Modes:**
  * Single simulation with detailed round-by-round console output.
  * Multiple simulations (e.g., 5000 iterations) to generate probabilities for tournament outcomes.
* **Data Management:** Tournament data and FIFA bracket matrices are stored in external `JSON` files.

## Tech Stack

* **Platform:** .NET Console Application
* **Language:** C#
* **Data Processing:** `System.Text.Json`, Generics (`<T>`)
* **Collections:** LINQ

## Project Structure

The solution follows a multi-layered architecture to separate concerns:

* **`BusinessLogic/`** — Core domain and application logic.
  * `Interfaces/` — Abstractions for dependency injection (`ITournamentRepository`, `ITournamentUI`).
  * `Models/` — Domain entities (Teams, Matches, Groups, etc.).
  * `Services/` — Business logic services (e.g., `DataMappingService`).
  * `Simulation/` — Core simulation algorithms (`MatchSimulator`, `TournamentSimulator`).
* **`Data/`** — Static JSON configuration files (`wc2026.json`, `fifa2026_third_place_matrix.json`).
* **`DataAccess/`** — Data retrieval and deserialization.
  * `DTOs/` — Data Transfer Objects for parsing JSON.
  * `Repositories/` — File reading and JSON deserialization implementation (`TournamentRepository`).
* **`Helpers/`** — Utility and extension methods.
* **`Presentation/`** — User interface layer.
  * `UI/` — Console output formatting and user input validation (`ConsoleUIService`).
* **`Program.cs`** — Application entry point and dependency wiring.

## How to Run

1. Clone the repository.
2. Open the solution (`.sln`) in Visual Studio or Rider.
3. Ensure the file paths for `wc2026.json` and `fifa2026_third_place_matrix.json` in the data access layer point to the correct local directories, or configure them to copy to the output directory.
4. Build and run the project.
5. Follow the console prompts to select the simulation mode.
