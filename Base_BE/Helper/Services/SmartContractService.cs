﻿using System.Numerics;
using Base_BE.Application.Common.Interfaces;
using Base_BE.Application.Dtos;
using Base_BE.Application.Vote.Commands;
using Base_BE.Dtos;
using Base_BE.Endpoints;
using Base_BE.Helper.key;
using Base_BE.Infrastructure.Data;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using BallotVoterDto = Base_BE.Dtos.BallotVoterDto;
using TransactionReceipt = Nethereum.RPC.Eth.DTOs.TransactionReceipt;

namespace Base_BE.Helper.Services
{

    public class SmartContractService
    {
        private readonly Web3 _web3;
        private readonly string contractAddress;
        private readonly ILogger<SmartContractService> _logger;
        private readonly string abi;

        public SmartContractService(IConfiguration configuration, ILogger<SmartContractService> logger)
        {

            var rpcUrl = configuration["Ethereum:RpcUrl"];
            var privateKey = configuration["Ethereum:PrivateKey"];
            var chainId = configuration.GetValue<long>("Ethereum:ChainId");
            abi = configuration["Ethereum:Abi"];
            contractAddress = configuration["Ethereum:ContractAddress"];
            _logger = logger;

            var credentials = new Account(privateKey, chainId);
            _web3 = new Web3(credentials, rpcUrl);
        }

        private Contract GetContract()
        {
            return _web3.Eth.GetContract(abi, contractAddress);
        }

        public async Task<TransactionReceipt> SubmitVoteAsync(SubmitVoteModel model)
        {
            try
            {
                var contract = GetContract();
                var giveVotingRightFunction = contract.GetFunction("giveVotingRight");

                _logger.LogInformation($"Submitting vote for VoterId={model.VoterId}, VoteId={model.VoteId}");


                TransactionReceipt receipt = await giveVotingRightFunction.SendTransactionAndWaitForReceiptAsync(
                    from: _web3.TransactionManager.Account.Address,
                    gas: new HexBigInteger(3000000),
                    value: null,
                    receiptRequestCancellationToken: default(CancellationToken),
                    functionInput: new object[]
                    {
                        model.BitcoinAddress,
                        model.Candidates.ToArray(),
                        model.VoterId,
                        model.VoteId,
                        DateTime.UtcNow.ToString("o")
                    }

                );

                return receipt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting vote");
                throw;
            }
        }

        public async Task<int> CountBallotForCandidateAsync(string candidateId, string voteId)
        {
            try
            {
                var contract = GetContract();
                var function = contract.GetFunction("countBallotForCandidate");

                _logger.LogInformation($"Counting ballots for CandidateId={candidateId}, VoteId={voteId}");

                var result = await function.CallAsync<BigInteger>(candidateId.ToString(), voteId.ToString());
                return (int)result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting ballots");
                throw;
            }
        }

        public async Task<BallotVoterDto> GetBallotVoterAsync(string address)
        {
            try 
            {
                // Lấy hợp đồng từ hàm GetContract
                var contract = GetContract();
                var function = contract.GetFunction("getBallotVoterData");

                // Gọi hàm getBallotVoterData từ hợp đồng
                var result = await function.CallAsync<Tuple<
                    BigInteger,        // Id
                    List<string>,      // CandidateIds
                    string,            // VoterId
                    string,            // VoteId
                    string,            // VotedTime
                    string             // BitcoinAddress
                >>(address);

                // Trả về DTO chứa dữ liệu đã xử lý
                return new BallotVoterDto
                {
                    Id = result.Item1.ToString(),
                    CandidateIds = result.Item2,
                    VoterId = result.Item3,
                    VoteId = result.Item4,
                    VotedTime = DateTime.TryParse(result.Item5, out var votedTime)
                        ? votedTime
                        : default,
                    BitcoinAddress = result.Item6
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching ballot data for voter address: {address}");
                throw;
            }
        }



        public async Task<bool> CheckExistBallotAsync(string voteId, string voterId)
        {
            try
            {
                var contract = GetContract();
                var function = contract.GetFunction("checkExistBallot");

                _logger.LogInformation($"Checking if ballot exists for VoteId={voteId}, VoterId={voterId}");

                var result = await function.CallAsync<bool>(voteId.ToString(), voterId.ToString());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of ballot");
                throw;
            }
        }
    }
}
