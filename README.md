# 📦 code-snippets

A personal C# / .NET learning repository containing practical implementations of **SOLID principles**, **design patterns**, and **small-scale architectural concepts**. 
This repo serves as a hands-on reference and sandbox for software engineering best practices.

***

## 📁 Repository Structure

```
code-snippets/
├── DesignPatterns/         # Implementations of classic GoF and modern design patterns
├── OrderProcessing/        # Domain example showcasing order lifecycle and processing logic
├── PaymentProcessing/      # Payment workflows including CheckoutService with multiple approaches
├── ProgramSnippets/        # Standalone console programs and isolated concept demos
├── .gitignore
└── code-snippets.sln       # Visual Studio solution file
```

***

## 🧩 Design Patterns

Located in `/DesignPatterns`, this section covers creational, structural, and behavioral patterns with real-world-inspired examples.

| Category | Patterns Covered |
|----------|-----------------|
| Creational | Factory Method, Abstract Factory, Builder, Singleton |
| Structural | Decorator, Adapter, Composite |
| Behavioral | Strategy, Observer, Command |

> Each pattern is implemented as a self-contained example, often using domain contexts like **payment gateways**, **order handling**, or **notification systems** to make the concepts concrete and relatable.

***

## 🛒 Order Processing

Located in `/OrderProcessing`, this module demonstrates:

- Order lifecycle management (creation → validation → fulfillment)
- Domain modeling with clean separation of concerns
- Application of SOLID principles in a realistic domain context

***

## 💳 Payment Processing

Located in `/PaymentProcessing`, this module includes:

- `CheckoutService` with multiple dynamic pricing and checkout approaches
- Strategy pattern applied to payment method selection
- Extensible design for adding new payment providers

***

## 🔬 Program Snippets

Located in `/ProgramSnippets`, this section contains:

- Isolated console applications for quick concept testing
- Demonstrations of C# language features and .NET APIs
- Experimental implementations before they mature into structured modules

***

## 🏗️ Tech Stack

- **Language:** C# (.NET)
- **IDE:** Visual Studio / VS Code
- **Build:** .NET CLI / MSBuild

***

## 🎯 Purpose

This repository is not a production system — it is a **deliberate practice space** for:

- Reinforcing SOLID principles through repeated application
- Building an intuitive feel for when and how to apply design patterns
- Experimenting with architectural ideas before applying them in larger systems

***

## 🚀 Getting Started

```bash
# Clone the repository
git clone https://github.com/jasimahmedkhan/code-snippets.git

# Open the solution
cd code-snippets
dotnet build code-snippets.sln
```

Navigate into any subfolder and run the relevant project:

```bash
cd DesignPatterns
dotnet run --project <ProjectName>
```

***

## 👤 Author

**Jasim Ahmed Khan**
Backend Software Engineer — passionate about clean architecture, cloud-native systems, and continuous learning.
