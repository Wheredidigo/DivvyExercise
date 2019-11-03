/********************************************************
                      DynamoDb
 ********************************************************/
variable "dynamoDB_table_name" {
  default = "parsed_from_data_ingest"
}
variable "dynamoDB_hash_key" {
  default = "name"
}


/********************************************************
                      Account Maps
 ********************************************************/
variable "environment_acct_map" {
  description = "Provide a default account mapping for prod/qa/dev etc"
  type        = "map"

  default = {
    PROD  = "000000000000"
    SMOKE = "111111111111"
    STAGE = "222222222222"
    QA    = "333333333333"
    DEV   = "444444444444"
  }
}

locals {
  account_id = "${data.aws_caller_identity.current_identity.account_id}"
  region     = "${data.aws_region.current.name}"
  is_prod    = "${local.account_id == var.environment_acct_map["PROD"]}"
  is_smoke   = "${local.account_id == var.environment_acct_map["SMOKE"]}"
  is_stage   = "${local.account_id == var.environment_acct_map["STAGE"]}"
  is_qa      = "${local.account_id == var.environment_acct_map["QA"]}"
  is_dev     = "${local.account_id == var.environment_acct_map["DEV"]}"
  common_tags = {
    app_name    = "data_exercise"
    owner_email = "data@exercise.com"
    owner_name  = "data exercise"
  }
}
