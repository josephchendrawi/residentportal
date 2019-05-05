alter table P_ACCNT
add REFERRED_BY bigint

alter table P_USER
drop column referred_by