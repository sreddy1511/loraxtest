using Amazon.SQS;
using Amazon.SQS.Model;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace SqsTests
{
    [TestFixture]
    public class SqsTest
    {
        private ISqsService _sqsService;
        private string _queueUrl;
        private const string QueueName = "TestSqsQueue";
        private const string AwsRegion = "us-east-1"; // Adjust as needed

        [SetUp]
        public async Task Setup()
        {
            try
            {
                var sqsClient = new AmazonSQSClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));
                _sqsService = new SqsService(sqsClient);
                _queueUrl = await _sqsService.CreateQueueAsync(QueueName);
                Console.WriteLine($"Created queue: {_queueUrl}");
            }
            catch (AmazonSQSException ex)
            {
                Assert.Fail($"Setup failed: AWS SQS error - {ex.Message}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Setup failed: Unexpected error - {ex.Message}");
            }
        }

        [Test]
        public async Task SendAndReceiveMessage_ShouldMatchSentMessage()
        {
            // Arrange
            const string sentMessage = "Test message for SQS";
            string receiptHandle = null;

            try
            {
                // Act: Send message
                await _sqsService.SendMessageAsync(_queueUrl, sentMessage);

                // Act: Receive message
                var receivedMessage = await _sqsService.ReceiveMessageAsync(_queueUrl);
                Assert.IsNotNull(receivedMessage, "No message received from queue");

                // Get receipt handle for cleanup (simplified for this test)
                var receiveResponse = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MaxNumberOfMessages = 1,
                    WaitTimeSeconds = 5
                });
                if (receiveResponse.Messages.Count > 0)
                {
                    receiptHandle = receiveResponse.Messages[0].ReceiptHandle;
                }

                // Assert
                Assert.AreEqual(sentMessage, receivedMessage, "Received message does not match sent message");

                // Cleanup: Delete message
                if (!string.IsNullOrEmpty(receiptHandle))
                {
                    await _sqsService.DeleteMessageAsync(_queueUrl, receiptHandle);
                    Console.WriteLine("Message deleted from queue.");
                }
            }
            catch (AmazonSQSException ex)
            {
                Assert.Fail($"Test failed: AWS SQS error - {ex.Message}");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed: Unexpected error - {ex.Message}");
            }
        }

        [TearDown]
        public async Task Cleanup()
        {
            try
            {
                await _sqsService.DeleteQueueAsync(_queueUrl);
                Console.WriteLine("Queue deleted.");
            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine($"Cleanup warning: Failed to delete queue - {ex.Message}");
            }
            finally
            {
                (_sqsService as IDisposable)?.Dispose();
            }
        }

        private IAmazonSQS _sqsClient => (_sqsService as SqsService)?._sqsClient; // Helper for receipt handle access
    }
}
