resource "aws_sns_topic" "data_ingest_notifications" {
  name = "data_ingest_notifications"
  tags = "${local.common_tags}"
}

resource "aws_sns_topic_policy" "data_ingest_notifications_policy" {
  arn = "${aws_sns_topic.data_ingest_notifications.arn}"

  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Id": "SNS Policy",
  "Statement": [
    {
      "Sid": "Allow Publish from S3",
      "Effect": "Allow",
      "Principal": {
        "Service": "s3.amazonaws.com"
      },
      "Action": "SNS:Publish",
      "Resource": "${aws_sns_topic.data_ingest_notifications.arn}",
      "Condition": {
        "ArnLike": {
          "aws:SourceArn": "${aws_s3_bucket.data_ingest_bucket.arn}"
        }
      }
    },
    {
      "Sid": "Allow SQS to subscribe",
      "Effect": "Allow",
      "Principal": {
        "Service": "sqs.amazonaws.com"
      },
      "Action": [
        "SNS:Subscribe",
        "SNS:Receive"
      ],
      "Resource": "${aws_sns_topic.data_ingest_notifications.arn}"
    }
  ]
}
POLICY
}
