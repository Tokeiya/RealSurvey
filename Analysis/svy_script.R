svy_relevel_generation<-function(.data)
{
  tmp<-.data
  tmp$generation<-fct_relevel(tmp$generation,'Nehalem','Sandy Bridge','Ivy Bridge','Haswell','Haswell Refresh',
                       'Broadwell','Skylake','Kaby Lake','Apollo Lake','Coffee Lake','Coffee Lake Refresh','Zen')
  
  tmp$process_architecture<-fct_relevel(tmp$process_architecture,"X86","X64")
  
  
  return(tmp)
}

svy_create_markdown<-function(.con)
{
  tmp<-dbGetQuery(.con,'SELECT * FROM session') %>% select(generation,cpu,process_architecture,os,framework) %>% 
    mutate(generation=as_factor(generation)) %>% mutate(process_architecture=as_factor(process_architecture));
  
  tmp<-svy_relevel_generation(tmp);
  
  tmp<-tmp %>% arrange(process_architecture,generation);
  
  kable(tmp,format='markdown')
  
}

create_cross_list<-function(.path)
{
  
  tmp<-read_tsv(.path) %>% mutate(f_env=str_c(first_order,':',first_environment)) %>% 
    mutate(s_env=str_c(second_order,':',second_environment)) %>%
    select(second_order,f_env,s_env,result) %>%
    spread(f_env,result) %>% mutate(s_env=map_chr(s_env,~str_remove(.,'\\d\\d:'))) %>%
    select(-second_order);
  
  names(tmp)<-names(tmp) %>% str_remove('\\d\\d:');
  
  return(tmp)
  c
  
}