resource "aws_sqs_queue_policy" "data_file_events_policy" {
  queue_url = "${aws_sqs_queue.data_file_events.id}"

  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Id": "l0 notifications",
  "Statement": [
      {
          "Sid": "Allow SNS to send S3 notifications",
          "Effect": "Allow",
          "Principal": {
              "AWS":"*"
          },
          "Action": "SQS:SendMessage",
          "Resource": "${aws_sqs_queue.data_file_events.arn}",
          "Condition": {
              "ArnEquals": { "aws:SourceArn": "${aws_sns_topic.data_ingest_notifications.arn}"}
          }
      },
      {
          "Sid": "Allow Lambda to get messages",
          "Effect": "Allow",
          "Principal": {
              "Service": "lambda.amazonaws.com"
          },
          "Action": [
            "lambda:CreateEventSourceMapping",
            "lambda:ListEventSourceMappings",
            "lambda:ListFunction"
          ],
          "Resource": "${aws_sqs_queue.data_file_events.arn}"
      }
  ]
}
POLICY
}
