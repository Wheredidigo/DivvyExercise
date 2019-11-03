resource "aws_lambda_function" "lambda_function" {
  s3_bucket     = "${aws_s3_bucket.lambda.id}"
  s3_key        = "${aws_s3_bucket_object.lambda_artifact.id}"
  function_name = "data-ingest-handler"
  role          = "${aws_iam_role.lambda_role.arn}"
  handler       = "DivvyExercise.Lambda::DivvyExercise.Lambda.Function::FunctionHandler"
  runtime       = "dotnetcore2.1"
  memory_size   = 512
  timeout       = 30

  /*  We could look at passing in specific environment variables here
  environment = {
    variables = {
      ASPNETCORE_ENVIRONMENT = "${local.is_prod  ? "Production"  :
                                  local.is_smoke ? "Smoke"       :
                                  local.is_stage ? "Staging"     :
                                  local.is_qa    ? "QA"          :
                                  local.is_dev   ? "Development" : "N/A"
      }"
    }
  }
*/
  tags = "${local.common_tags}"
}

resource "aws_lambda_permission" "sqs_permission" {
  statement_id  = "Allow_Execution_From_SQS"
  action        = "lambda:InvokeFunction"
  function_name = "${aws_lambda_function.lambda_function.arn}"
  principal     = "sqs.amazonaws.com"
  source_arn    = "${aws_sqs_queue.data_file_events.arn}"
}

resource "aws_lambda_event_source_mapping" "event_source_mapping" {
  event_source_arn = "${aws_sqs_queue.data_file_events.arn}"
  enabled          = true
  function_name    = "${aws_lambda_function.lambda_function.arn}"
  batch_size       = "10"
}