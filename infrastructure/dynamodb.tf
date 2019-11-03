resource "aws_dynamodb_table" "main" {
  name             = "${var.dynamoDB_table_name}"
  billing_mode     = "PAY_PER_REQUEST"
  hash_key         = "${var.dynamoDB_hash_key}"
  stream_enabled   = true
  stream_view_type = "NEW_AND_OLD_IMAGES"

  server_side_encryption {
    enabled = true
  }

  point_in_time_recovery {
    enabled = true
  }

  attribute {
    name = "${var.dynamoDB_hash_key}"
    type = "S"
  }

  tags = "${local.common_tags}"
}

resource "aws_dynamodb_table" "backup" {
  provider = "aws.us-east-1"

  name             = "${var.dynamoDB_table_name}"
  billing_mode     = "PAY_PER_REQUEST"
  hash_key         = "${var.dynamoDB_hash_key}"
  stream_enabled   = true
  stream_view_type = "NEW_AND_OLD_IMAGES"

  server_side_encryption {
    enabled = true
  }

  point_in_time_recovery {
    enabled = true
  }

  attribute {
    name = "${var.dynamoDB_hash_key}"
    type = "S"
  }

  tags = "${local.common_tags}"
}

resource "aws_dynamodb_global_table" "global" {
  depends_on = ["aws_dynamodb_table.main", "aws_dynamodb_table.backup"]
  name       = "${var.dynamoDB_table_name}"

  replica {
    region_name = "us-west-2"
  }

  replica {
    region_name = "us-east-1"
  }
}
