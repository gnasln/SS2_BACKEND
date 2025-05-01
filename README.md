# Blockchain-Based Voting System: A Secure and Transparent Electoral Solution
## Overview
Blockchain-Based Voting System is a secure and transparent digital platform designed to revolutionize the electoral process by leveraging blockchain technology. The application addresses the limitations of traditional paper-based voting systems (resource waste, logistical challenges, security concerns) and improves upon conventional e-voting systems by implementing an immutable, decentralized ledger that ensures vote integrity and prevents manipulation. Built with Angular JS for the frontend and ASP.NET Core for the backend, the system operates on the Polygon network to provide high throughput and cost-effective transactions while maintaining environmental efficiency.

The platform features comprehensive user authentication with multi-factor verification, secure voter registration, blockchain-based ballot creation, anonymous yet verifiable vote casting, real-time vote tracking, transparent counting mechanisms, and an intuitive analytics dashboard. By combining cryptographic security with blockchain's inherent transparency, our solution aims to increase electoral trust, reduce administrative costs, and enhance accessibility while maintaining the highest standards of vote privacy and system integrity. The system enables organizations to conduct secure elections where results can be independently verified without compromising voter anonymity, ultimately strengthening democratic processes through technological innovation.


## Table of Contents
1. [Overview](https://github.com/gnasln/SS2_BACKEND/#overview)
2. [Technology Explanation](https://github.com/gnasln/SS2_BACKEND/#technology-explanation)
3. [Functions](https://github.com/gnasln/SS2_BACKEND/#functions)
4. [Database Inintialization](https://github.com/gnasln/SS2_BACKEND/#database-initialization)

## Technology Explanation

For Back-end design, this project was generated with ASP.NET Core.

Blockchain Technology in the Voting System

The voting system leverages blockchain technology through the Polygon network (formerly Matic), a layer 2 scaling solution for Ethereum that provides high throughput, low transaction fees, and environmental efficiency. This implementation delivers the core benefits of blockchain—immutability, transparency, and security—while overcoming traditional blockchain limitations related to transaction speed and cost.

![Screenshot 2025-05-01 103802](https://github.com/user-attachments/assets/48b70355-c4d0-4c03-9a57-ebef64ac42a9)

Core Blockchain Components
Distributed Ledger Architecture
- The system uses a distributed ledger where each vote is stored as a transaction across multiple nodes, eliminating single points of failure and creating a tamper-resistant record. Unlike centralized databases, this distributed architecture ensures no single entity can modify votes once cast.

Smart Contracts
The system implements Solidity-based smart contracts that govern the entire voting process:
- Ballot Creation Contracts: Define election parameters, candidate information, and voting periods
- Vote Verification Contracts: Validate voter eligibility and prevent double-voting
- Vote Counting Contracts: Automatically tally results using predefined rules
- Result Certification Contracts: Autonomously certify election outcomes when predetermined conditions are met

Cryptographic Security
The system employs advanced cryptographic techniques to:
- Secure voter identities through asymmetric encryption
- Create anonymous yet verifiable voting records
- Generate unique transaction IDs that allow voters to track their votes
- Implement zero-knowledge proofs to verify vote validity without revealing voter choices

Consensus Mechanism
The Polygon network uses a Proof of Stake (PoS) consensus mechanism, which:
- Validates transactions efficiently with minimal environmental impact
- Provides faster transaction finality compared to Proof of Work systems
- Ensures agreement across the network regarding the validity of each vote

Integration Architecture
The blockchain layer integrates with the application through:
- Web3.js Integration: Connects the Angular JS frontend directly to the Polygon blockchain
- Smart Contract Interfaces: Facilitates communication between the ASP.NET Core backend and deployed contracts
- Event Listeners: Captures and processes blockchain events for real-time updates
- Transaction Management: Handles the submission and confirmation of voting transactions

![Screenshot 2025-05-01 103918](https://github.com/user-attachments/assets/3f81b322-ed57-40e8-9ec6-47d476a26025)

Benefits of This Blockchain Implementation

- Immutable Audit Trail: Every vote creates a permanent, unchangeable record on the blockchain
- Transparency: The entire process can be independently verified without compromising voter privacy
- Elimination of Trust Requirements: The system doesn't require trusted third parties to validate results
- Resistance to Manipulation: Distributed architecture prevents vote tampering and fraud
- Real-time Verification: Voters can confirm their votes were recorded correctly
- Cost Efficiency: Polygon's low transaction fees make large-scale elections economically viable

![Screenshot 2025-05-01 104008](https://github.com/user-attachments/assets/05ec1d08-da1e-45a8-ac0f-69ac01f895fa)

## Functions
(The following list is unsorted)
- Login
- Personal Information
- User Management
- Statistic Management
- Notifications
- Voting Management
- Hierarchy Management
- Password Recovery
- Voting Function
- Account Management
- Database Management

## Database Initialization
Full Project Initialization
![z6518242810172_39a3cb59cdfc7c47be4cc6b508b6d20f](https://github.com/user-attachments/assets/cc22b8fa-6eee-49c6-bd6c-e972bc0d522a)

User Management
![z6557003151095_5c42959a0c5e6ccb2f9e78ca12d71887](https://github.com/user-attachments/assets/21492207-e85d-4db3-bdd3-3e92f2a72859)

**---Work In Progress---**
