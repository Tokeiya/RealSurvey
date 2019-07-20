loadLib<-function()
{
  library(RSQLite);
  library(knitr);
  library(kableExtra);
  library(formattable);
  
  library(tidyverse);
  library(forcats);
  
}

get_count<-function(.data)
{
  map_int(.data,~count(.)[[1]]);
}

relevel_category<-function(.category)
{
  fct_relevel(.category,'x86Windows','x64Windows','x64WindowsFMA3','Linux','macOS');
}