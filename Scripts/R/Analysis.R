library(readxl)
library(dplyr)

filename <- "C:/Users/Administrator/Desktop/DataSet/1213_04/Prosaccade_cd_04_A.csv"

data <- read.csv(filename)

# 分组（这里的分组是将index为1或者-1的时候进行分组）
data_group <- data %>%
  # 标记是否为新组的起点 (0 开头，且后面不是0，为1或者是-1)
  mutate(group_start = ifelse(index == 0 & lead(index) != 0, 1, 0)) %>%
  mutate(group = cumsum(group_start)) %>%
  # 去掉所有组以0结束的行（每一组的最后不以0结束）
  group_by(group) %>%
  filter(!(index == 0 & all(tail(index, 1) == 0))) %>%
  ungroup()
#View(data_group)

# 筛选 --- > prosaccade情况
result <- data_group %>%
  # 按分的组进行处理
  group_by(group) %>%
  group_modify(~ {
    # 计算连续 IN_SACCADE == 1 的行
    rle_saccade <- rle(.x$IN_SACCADE == 1)
    lengths <- rle_saccade$lengths
    values <- rle_saccade$values
    
    # 找到符合条件的连续的行
    keep_indices <- which(values & lengths >= 3)
    if (length(keep_indices) == 0) {
      # 如果没有长度 >= 3 的片段，寻找长度 >= 2 的片段
      keep_indices <- which(values & lengths >= 2)
    }
    if (length(keep_indices) > 0) {
      # 提取符合条件的行
      row_ranges <- unlist(
        lapply(keep_indices, function(i) {
          start <- sum(lengths[1:(i - 1)]) + 1  # 当前片段的起始行
          end <- start + lengths[i] - 1        # 当前片段的结束行
          seq(start, end)
        })
      )
      .x[row_ranges, ]  # 仅保留这些行
    } else {
      # 如果没有任何符合条件的片段，则返回空（这个可以认为是没有进行移动了）
      .x[0, ]
    }
  }) %>%
  ungroup()


# 标记需要保留的 index 为 0 的行
data <- data %>%
  mutate(keep = ifelse(index == 0 & (lead(index) == 1 | lead(index) == -1), TRUE, FALSE)) %>%
  filter(index == 1 | index == -1 | keep)
#View(data)

# 标记连续的 1
data <- data %>%
  mutate(group = cumsum(c(0, diff(IN_SACCADE) == -1)))
#View(data)

# 数据取交集，取相同的时间戳
intersection_data <- data %>% semi_join(result, by = "TimeStamp")

# 保留 index 为 0 的行及其下一行
data_with_index_zero <- data %>%
  mutate(keep = index == 0 | lag(index, default = NA) == 0) %>% 
  filter(keep) %>%
  select(-keep)

# 合并交集数据和保留 index 为 0 的行
combined_data <- bind_rows(intersection_data, data_with_index_zero) %>%
  distinct(TimeStamp, .keep_all = TRUE) %>%
  arrange(TimeStamp)

# 分组计算时间戳差值
data_with_groups <- combined_data %>%
  mutate(group_id = cumsum(index == 0)) %>%
  group_by(group_id) %>% 
  filter(n() >= 3) %>%
  arrange(group_id, TimeStamp) %>% 
  mutate(timestamp_diff = TimeStamp[3] - TimeStamp[2]) %>%
  filter(row_number() == 2) %>% 
  pull(timestamp_diff)

# 输出结果
timestamp_diffs <- data_with_groups
print(timestamp_diffs)








# 对于antisaccade情况

filename <- "C:/Users/Administrator/Desktop/DataSet/1213_04/Antisaccade_cd_04_A.csv"

data <- read.csv(filename)

# 分组（这里的分组是将index为1或者-1的时候进行分组）
data_group <- data %>%
  # 标记是否为新组的起点 (0开头，且后面不是0，为1或者是-1)
  mutate(group_start = ifelse(index == 0 & lead(index) != 0, 1, 0)) %>%
  mutate(group = cumsum(group_start)) %>%
  # 去掉所有组以0结束的行（每一组的最后不以0结束）
  group_by(group) %>%
  filter(!(index == 0 & all(tail(index, 1) == 0))) %>%
  ungroup()
#View(data_group)

# 标记需要保留的 index 为 0 的行
data <- data %>%
  mutate(keep = ifelse(index == 0 & (lead(index) == 1 | lead(index) == -1), TRUE, FALSE)) %>%
  filter(index == 1 | index == -1 | keep)
#View(data)

# 标记连续的 1
data <- data %>%
  mutate(group = cumsum(c(0, diff(IN_SACCADE) == -1)))
#View(data)


# 对于antisaccade筛选的逻辑
result <- data %>%
  # 筛选 IN_SACCADE 为 1 且 index 不为 0 的行
  filter(IN_SACCADE == 1, index != 0) %>% 
  # 对上面分好的组进行分组
  group_by(group) %>%  
  # 筛选连续的 IN_SACCADE 为 1 的行
  filter(sum(IN_SACCADE == 1) >= 3) %>%
  filter(
    # 检查是否整组的点都满足条件且至少有 3 个点 --- >这里的逻辑有问题
    !(n() >= 3 && all((GazeX - TargetPositionX) * GazeX < 0))
  ) %>%
  ungroup()
# print(result)

# 数据取交集，取相同的时间戳
intersection_data <- data %>% semi_join(result, by = "TimeStamp")

# 保留 index 为 0 的行及其下一行
data_with_index_zero <- data %>%
  mutate(keep = index == 0 | lag(index, default = NA) == 0) %>% 
  filter(keep) %>%
  select(-keep)

# 合并交集数据和保留 index 为 0 的行
combined_data <- bind_rows(intersection_data, data_with_index_zero) %>%
  distinct(TimeStamp, .keep_all = TRUE) %>%
  arrange(TimeStamp)

# 分组计算时间戳差值
data_with_groups <- combined_data %>%
  mutate(group_id = cumsum(index == 0)) %>%
  group_by(group_id) %>% 
  filter(n() >= 3) %>%
  arrange(group_id, TimeStamp) %>% 
  mutate(timestamp_diff = TimeStamp[3] - TimeStamp[2]) %>%
  filter(row_number() == 2) %>% 
  pull(timestamp_diff)

# 输出结果
timestamp_diffs <- data_with_groups
print(timestamp_diffs)





