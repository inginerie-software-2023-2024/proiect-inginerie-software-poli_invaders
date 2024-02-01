# Software Architecture Report

**Date:** February 1, 2024

**Version:** v1.1 (Commit ID: 298c483)

## **Contents:**

### **1. Purpose of the Software Project**

The project aims to create an engaging multiplayer game that provides users with the flexibility to choose between melee and ranged gameplay. The storyline involves a brave student defending against invaders and seeking high-tech gadgets. The game features endless modes, level progression, remote multiplayer, power-ups, character customization, and various challenges.

### **2. Fulfilled Capabilities**

The project, in its current state, allows users to start and play the game, move characters, defeat enemies, and access both local and remote multiplayer modes. The core functionalities include endless mode, level progression, and basic multiplayer interaction. However, some planned features, such as gameplay type selection (melee/ranged), are yet to be implemented.

### **3. Guides**

### 3.1. Run the Project Locally

Clone the project from the server repository([https://github.com/stefanmoldoveanu23/poli-server](https://github.com/stefanmoldoveanu23/poli-server)) and from the game repository([https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-poli_invaders](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-poli_invaders)). Follow the instructions in the README file to set up and run the server. Download and install Unity Hub and Unity Editor. Open the Unity project, ensuring compatibility with the committed version.

### 3.2. Build the Project

Follow the instructions in the README file for building the project locally. Unity's build settings can be configured to generate platform-specific builds.

### 3.3. Deploy the Project

For local deployment, Unity's built-in deployment options can be used. For remote deployment, follow the instructions provided in the server repository README.

### **4. Contribution Guide**

Refer to the CONTRIBUTING.md file for guidelines on contributing to the project. It covers coding standards, branch naming conventions, and the pull request process.

### **5. Patterns Used in the Application**

The project follows the Model-View-Controller (MVC) pattern for managing game logic and user interface. Observer pattern is employed for multiplayer interactions.

### **6. Application Entry Points**

Main entry points include the game initialization, scene loading, and multiplayer connections. Refer to the project's codebase for specific details.

### **7. Data Sources**

The main data sources are the game server, responsible for multiplayer coordination, and Firebase, which stores leaderboard data.

### **8. Data Inputs**

User inputs mouse actions for character movement, attacks, and interactions. Multiplayer interactions involve data exchange with the game server.

### **9. Configuration Files**

Unity's configuration files and settings are utilized for project configuration. Refer to the Unity documentation for customization.

### **10. High-Level Diagrams of the Architecture**

Refer to the included high-level architecture diagram (Architecture_Diagram.png) for an overview of the project's structure.

### **11. User/Data Journeys**

Detailed user journeys and data flows are provided in the user journey map and activity diagram images.

### **12. Most Valuable Output**

The high score system, the leaderboard functionality, the multiple levels structure and the real time multiplayer functionality are highlighted as the most valuable outputs, providing users with a competitive and engaging experience and offering a way to enjoy the game remotely, in a co-op version.

### **13. Deployment Plan**

The application is currently deployed locally for testing purposes. Future deployments may involve cloud services or dedicated servers based on project needs.

### **14. CI/CD Pipeline**

The CI/CD pipeline is implemented using GitHub Actions. There are workflows for the game itself and for the server where the multiplayer is hosted. Both repositories have Actions that build the project, however, the game repository has a semi-functional Action that verifies if the game’s tests are passing. The Actions were written using various repositories such as: 

for the Server → GitHub’s official repo for Actions and Google’s Cloud official Actions

for the Game → A free, open-source library specifically meant for Unity CI: Game-CI and GitHub’s official repo for Actions

### **15. QA Process**

The QA process heavily relied onto manual testing. A comprehensive automated test suite is planned for future development. Types of manual testing:

White Box Techniques: 

**Branch Software Testing →** It ensures that no branch leads to abnormal behavior of the application

Black Box Techniques:

**Unit Testing →** It isolates sections of code and tests every function for its correctness 

Usability Testing:

Evaluate a product by testing it with the proper users

Regression Testing: 

It ensures that newly added features won’t affect the already implemented ones

Visual Testing:

It validates whether the developed software user interface (UI) is compatible with the user’s view

A report containing the user testing feedback can be found at UserReport.md

### **16. Test Suites**

Current test suites cover basic gameplay functionality, multiplayer interactions, and UI elements. Future iterations will expand test coverage for Play Mode and for Edit Mode as well. 

### **17. External Dependencies**

### 17.1. APIs Used

- Firebase API for leaderboard data storage.

### 17.2. Libraries

- Unity game engine libraries.
- Additional dependencies listed in the project's package manager.

### **18. Vulnerability to Dependency Attacks**

The project is aware of potential dependency attacks such as Dependency Confusion. Dependency versions are carefully managed, and the team regularly reviews and updates dependencies to mitigate security risks.
