resource "aws_sqs_queue" "data_file_events_dlq" {
  name = "data_file_events_dlq"
  tags = "${local.common_tags}"
}

resource "aws_sqs_queue" "data_file_events" {
  name                       = "data_file_events"
  visibility_timeout_seconds = 60

  redrive_policy = <<RDP
{
    "maxReceiveCount": 5,
    "deadLetterTargetArn": "${aws_sqs_queue.data_file_events_dlq.arn}"
}
RDP

  tags = "${local.common_tags}"
}
