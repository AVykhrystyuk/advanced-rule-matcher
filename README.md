# test-task: advanced-rule-matcher

###  Description

There is set of rules with following structure: RuleId, Rule Priority, Filter1.. Filter4.
| RuleId | Priority | Filter1 | Filter2 | Filter3 | Filter4 |
|--------|----------|---------|---------|---------|---------|
| 1      | 80       | AAA     | `<ANY>` | CCC     | DDD     |
| 2      | 10       | `<ANY>` | `<ANY>` | AAA     | `<ANY>` |
| 3      | 70       | BBB     | `<ANY>` | CCC     | `<ANY>` |
| 4      | 100      | AAA     | BBB     | CCC     | `<ANY>` |
| 5      | 50       | CCC     | AAA     | `<ANY>` | CCC     |
| 6      | 0        | `<ANY>` | `<ANY>` | `<ANY>` | `<ANY>` |

Purpose of the library to find best matching rules for datasets with filtering values (Filter1, ..., Filter4).
-  If record matches multiple rules, rule with highest priority must be selected (100 – highest priority, 0 – lowest priority). 
- Value `<ANY>` means, that any value is accepted for this filter.

Initially rules are stored in [CSV file](/AdvancedRuleMatcher/assets/SampleData.csv).

In real production environment quantity of rules expected to be 1000+. Quantity of data to pass through filter is 10x-100x larger or might be even infinity data stream in future.

### Must have features:
- To load data from a file to array of **strongly typed entities**
- To create a library that will be finding best matching rule (respecting rule priority) for the set of input values
- To unit tests the library

Sample to describe idea:
```csharp
Rule? rule = searchEngine.FindRule(string val1, string val2, string val3, string val4);
```

Expected, that this module will be used to find matching rules with the frequency 100 calls/second over loaded dataset.

### Should have features:
- Taking into account that collection can store 1000+ records it should be performant enough.
- Code should be reusable as much as possible to implement different Rules models. For instance, to write another set of strongly typed rules with filters: `Filter1(int), Filter2(bool), Filter3(string).`


### Validation
In case of provided sample data (see a table in Description or [CSV file](/AdvancedRuleMatcher/assets/SampleData.csv)), expected results will be following:
| Filter1 | Filter2 | Filter3 | Filter4 | | RuleId |
|---------|---------|---------|---------|-|--------|
| AAA     | BBB     | CCC     | AAA     | |  4     |
| AAA     | BBB     | CCC     | DDD     | |  4     |
| AAA     | AAA     | AAA     | AAA     | |  2     |
| BBB     | BBB     | BBB     | BBB     | |  6     |
| BBB     | CCC     | CCC     | CCC     | |  3     |
