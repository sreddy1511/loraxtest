using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

namespace SqsTests
{
    public interface ISqsService
    {
        Task<string> CreateQueueAsync(string queueName);
        Task SendMessageAsync(string queueUrl, string message);
        Task<string> ReceiveMessageAsync(string queueUrl);
        Task DeleteMessageAsync(string queueUrl, string receiptHandle);
        Task DeleteQueueAsync(string queueUrl);
    }

    public class SqsService : ISqsService, IDisposable
    {
        private readonly IAmazonSQS _sqsClient;
        private bool _disposed;

        public SqsService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient ?? throw new ArgumentNullException(nameof(sqsClient));
        }

        public async Task<string> CreateQueueAsync(string queueName)
        {
            if (string.IsNullOrEmpty(queueName)) throw new ArgumentNullException(nameof(queueName));

            var response = await _sqsClient.CreateQueueAsync(new CreateQueueRequest
            {
                QueueName = queueName
            });
            return response.QueueUrl;
        }

        public async Task SendMessageAsync(string queueUrl, string message)
        {
            if (string.IsNullOrEmpty(queueUrl)) throw new ArgumentNullException(nameof(queueUrl));
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));

            await _sqsClient.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = message
            });
        }

        public async Task<string> ReceiveMessageAsync(string queueUrl)
        {
            if (string.IsNullOrEmpty(queueUrl)) throw new ArgumentNullException(nameof(queueUrl));

            var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = 5 // Long polling
            });

            return response.Messages.Count > 0 ? response.Messages[0].Body : null;
        }

        public async Task DeleteMessageAsync(string queueUrl, string receiptHandle)
        {
            if (string.IsNullOrEmpty(queueUrl)) throw new ArgumentNullException(nameof(queueUrl));
            if (string.IsNullOrEmpty(receiptHandle)) throw new ArgumentNullException(nameof(receiptHandle));

            await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = receiptHandle
            });
        }

        public async Task DeleteQueueAsync(string queueUrl)
        {
            if (string.IsNullOrEmpty(queueUrl)) throw new ArgumentNullException(nameof(queueUrl));

            await _sqsClient.DeleteQueueAsync(new DeleteQueueRequest
            {
                QueueUrl = queueUrl
            });
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _sqsClient?.Dispose();
                _disposed = true;
            }
        }
    }
}
