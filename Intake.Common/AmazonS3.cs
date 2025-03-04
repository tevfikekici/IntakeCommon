using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using System.Text.Json;
using System.Text.Encodings.Web;
using Serilog;
using Amazon.S3.Model;

namespace Intake.Common
{
    public static class AmazonS3
    {     

        public static async Task<GetObjectResponse> GetObjectResponseAsync(IAmazonS3 s3Client, string key, string bucket)
        {
            Log.Debug($"Trying to establish connection to get object {key} response from s3 bucket {bucket}");
            var request = new Amazon.S3.Model.GetObjectRequest()
            {
                BucketName = bucket,
                Key = key
            };
            try
            {
                var response = await s3Client.GetObjectAsync(request);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                {
                    Log.Error($"Getting object has failed with HttpStatus {response.HttpStatusCode}");
                    throw new Exception($"IntakeHttpStatus");
                }
                Log.Debug($"Getting object {key} from s3 bucket {bucket} was successful");
                return response;
            }
            catch (Exception e)
            {
                if (!e.Message.Equals("IntakeHttpStatus"))
                {
                    Log.Error($"Getting object has failed. Details: {e.Message}, {e}");
                }
                throw new Exception($"Failed to retrieve stream {key} from bucket: {bucket}");
            }       
        }

        public static async Task DeleteObjectAsync(IAmazonS3 s3Client, string key, string bucket)
        {
            Log.Debug($"Trying to establish connection to delete object {key} from s3 bucket {bucket}");
            var request = new Amazon.S3.Model.DeleteObjectRequest()
            {
                BucketName = bucket,
                Key = key
            };
            await s3Client.DeleteObjectAsync(request);
            Log.Debug($"Deleting object {key} from s3 bucket {bucket} was successful");
        }

        public static async Task<T> GetObjectAsync<T>(IAmazonS3 s3Client, string key, string bucket)
        {
            Log.Debug($"Trying to establish connection to get object {key} from s3 bucket {bucket}");
            using var response = await s3Client.GetObjectAsync(bucket, key);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                Log.Error($"Getting object has failed with HttpStatus {response.HttpStatusCode}");
            }
            var options = new JsonSerializerOptions();
            var result = JsonSerializer.Deserialize<T>(response.ResponseStream, options);
            Log.Debug($"Getting object {key} from s3 bucket {bucket} was successful");
            return result;
        }

        public static async Task<PutObjectResponse> SetObjectAsync<T>(T obj, IAmazonS3 s3Client, string key, string bucket)
        {
            await using var memoryStream = new MemoryStream();
            var writerOptions = new JsonWriterOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var jsonWriter = new Utf8JsonWriter(memoryStream, writerOptions);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            JsonSerializer.Serialize(jsonWriter, obj, options);
            await jsonWriter.FlushAsync();

            var request = new Amazon.S3.Model.PutObjectRequest()
            {
                BucketName = bucket,
                Key = key,
                InputStream = memoryStream
            };
            Log.Debug($"Trying to establish connection to set object {key} in s3 bucket {bucket}");
            var response = await s3Client.PutObjectAsync(request);
            Log.Information($"Setting object {key} in s3 bucket {bucket} was successful");
            return response;
        }
    }
}
