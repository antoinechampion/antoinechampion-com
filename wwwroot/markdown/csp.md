![A hand with a rubik's cube](/images/rubik.jpg =913x527)
*A Rubik's cube could be modeled as a constraint satisfaction problem [1]*

Photo by [NeONBRAND](https://unsplash.com/@neonbrand?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText) on [Unsplash](https://unsplash.com/?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText)

*Machine Learning* and *Deep Learning* are ongoing buzzwords in the industry. Branding ahead of functionalities led to Deep Learning being overused in many artificial intelligence applications.

This post will provide a quick grasp at constraint satisfaction, a powerful yet underused  approach which can tackle a large number of problems in AI and other areas of computer science, from logistics and scheduling to temporal reasoning and graph problems.

### Constraint solvers

Let's consider a factual and highly topical problem.

<center><h4>A pandemic is rising. Hospitals must organize quickly to treat ill people.</h4><br/></center>

The world needs an algorithm which matches infected people and hospitals together given multiple criterias such as severity of illness, patient age and location, hospital capacity and equipment, etc.

Many would say that a neural network would be the perfect fit for it: different configurations from a broad range of parameters that need to be reduced to a unique solution.  
However, there are downsides which would undermine such an approach:

 - The model needs to be trained, hence the need for historical data on previous cases,
 - A lot of time would be wasted cleaning and consolidating the dataset,
 - A variety of architectures would need to be tested with lengthy training sessions.

On the other hand, if formulated in terms of a boolean satisfiability problem, the situation wouldn't have any of the aforementioned downsides while still giving a sub-optimal solution in nondeterministic polynomial time (NP-complete problem), and without the need for any historical data.

*Disclaimer: the purpose of this post is to deliver a quick look at CSPs. Theory and problem formulation will be much overlooked. For a more rigorous approach, please refer to [2][3][4].*

### Abstracting the problem

This post will provide a *gentle introduction to constraint programming*, aiming to resolve this case study. This map of the pandemic (1) illustrates the output of our algorithm which will match infected people with hospitals. 

![Map of ill people and hospitals](/images/csp-map-anim-inv.gif =851x750)
*Fig. 1: Map of the pandemic, depicting infected people and hospitals*

There are several frameworks for constraint solving. [Google Optimization Tools (a.k.a., OR-Tools)](https://developers.google.com/optimization) is an open-source software suite for solving combinatorial optimization problems. Our problem will be modeled using this framework in Python.

## Parameters

For now, let's simplify the problem to 4 parameters <sup>(1)</sup>:

- Location of infected people
- Severity of infected people
- Location of hospitals
- Number of beds in each hospital


Let's define those parameters in python:

<?prettify?>
    # Number of hospitals
    n_hospitals = 3
    # Number of infected people
    n_patients = 200
    # Number of beds in every hospital
    n_beds_in_hospitals = [30,50,20]
    # Location of infected people -- random integer tuple (x,y)
    patients_loc = [(randint(0, 100), randint(0, 100)) for _ in range(n_patients)]
    # Location of hospitals -- random integer tuple (x,y)
    hospitals_loc = [(randint(0, 100), randint(0, 100)) for _ in range(n_hospitals)]  
    # Illness severity -- 1 = mild -> 5 = severe
    patients_severity = [randint(1, 5) for _ in range(n_patients)]


## Variables

A constraint satisfaction problem consists of a set of variables that must be assigned values in such a way that a set of constraints is satisfied.

- Let $I$ be the set of hospitals
- Let $J_i$ be the set of beds in hospital $i$
- Let $K$ be the set of patients.

Let $(x_{ijk})\_{(i,j,k)\in I\times J_i \times K}$ with $x_{ijk}  \in \\{0,1\\}$ be our indexed family of variables. $x_{ijk} = 1$ if in the hospital $i$, the bed $j$ is taken by the person $k$. In order to associate each bed of an hospital to an ill person, the goal is to find a set of variables $x_{ijk}$ that satisfies all of our constraints.

We can add those variables to our model:

<?prettify?>
    model = cp_model.CpModel()
    x = {}
    for i in range(n_hospitals):
      for j in range(n_beds_in_hospitals[i]):
        for k in range(n_patients):
          x[(i,j,k)] = model.NewBoolVar("x(%d,%d,%d)" % (i,j,k))

## Hard constraints

Hard constraints define a goal for our model. They are essential, if they are not resolved then the problem can't be tackled:

- There must be **at most** a single person in every bed,
- There must be **at most** a single bed assigned to every person.

Let's focus on the first hard constraint. For each bed $j$ in every hospital $i$:
- Either there is a unique patient $k$,
- Either the bed is empty.

Hence, it can be expressed the following way:

<p>
$$
\begin{equation}
\tag{$\mathcal{C}_1$}
\forall i \in I, \forall j \in J_i,
\begin{cases}
\begin{gathered}
\exists! k \in K, x_{ijk} = 1 \\
\text{or} \\
\forall k \in K, x_{ijk} = 0
\end{gathered}
\end{cases} 
\label{eqn:C1}
\end{equation}
$$
</p>

Our solver is a combinatorial optimization solver, it can process integer constraints only. Hence, $\mathcal{C}_1$ must be turned into an integer equation:

<p>
$$
\forall i \in I, \forall j \in J_i, \sum_{k \in K} x_{ijk} \leq 1
$$
</p>

This inequality can then be added to our model:

<?prettify?>
    # Each bed must host at most one person
    for i in range(n_hospitals):
      for j in range(n_beds_in_hospitals[i]):
        model.Add(sum(x[(i,j,k)] for k in range(n_patients)) <= 1)


Next, the second hard constraint: for every patient $k$:
- Either he is in a unique bed $j$ in a unique hospital $i$,
- Either he is at home.

<p>
$$
\begin{equation}
\tag{$\mathcal{C}_2$}
\forall k \in K,
\begin{cases}
\begin{gathered}
\exists! (i,j) \in I \times J_i, x_{ijk} = 1 \\
\text{or} \\
\forall i \in I, \forall j \in J_i, x_{ijk} = 0
\end{gathered}
\end{cases}
\label{eqn:C2}
\end{equation}
$$
</p>

In the same way, $\mathcal{C}_2$ can be translated into an integer inequality:

<p>
$$
\forall k \in K, \sum_{i \in I} \sum_{j \in I_j} x_{ijk} \leq 1
$$
</p>

Finally, this constraint can be added to the model.

<?prettify?>
    # Each person must be placed in at most one bed
    for k in range(n_patients):
      inner_sum = []
      for i in range(n_hospitals):
        inner_sum.append(sum(x[(i,j,k)] for j in range(n_beds_in_hospitals[i]))) 
      model.Add(sum(inner_sum) <= 1)


## Soft constraints

Next, there are *soft constraints*. Those are highly desired: our solution must try to satisfy them as much as possible, yet they are not essential to find a solution:

- Every sick person *should* be placed into a bed,
- Every person *should* be handled by the nearest hospital,
- Sick persons in a severe condition *should* be handled first when there are not enough beds. 

While hard constraints are modeled as equalities or inequalities, soft constraints are expressions to minimize or maximize.

Let $\Omega = \\{ (x_{ijk}) \\ | \\ \mathcal{C_1}, \mathcal{C_2} \\}$ be the set of all solutions that satify the hard constraints.

*Every sick person should be placed into a bed* means to maximize the number of occupied beds.

<p>
$$
\begin{equation}
\tag{$\mathcal{C}_3$}
\max_\Omega \sum_{i \in I} \sum_{j \in I_j} \sum_{k \in K} x_{ijk}
\label{eqn:C3}
\end{equation}
$$
</p>

*Every person should be handled by the nearest hospital* means to minimize the distance between every patient and his assigned hospital.

<p>
$$
\begin{equation}
\tag{$\mathcal{C}_4$}
\min_\Omega \sum_{i \in I} \sum_{j \in I_j} \sum_{k \in K} x_{ijk} \\\ d(i,k)
\label{eqn:C4}
\end{equation}
$$
</p>

*Sick persons in a severe condition should be handled first when there are not enough beds* means to maximize the total severity of all handled patients. By denoting $\text{sev}(k)$ the severity of the patient $k$:

<p>
$$
\begin{equation}
\tag{$\mathcal{C}_5$}
\max_\Omega \sum_{i \in I} \sum_{j \in I_j} \sum_{k \in K} x_{ijk} \\\ \text{sev}(k)
\label{eqn:C5}
\end{equation}
$$
</p>

Then we can reduce all the soft constraints into a single objective:

<p>
$$
\mathcal{C}_3 \wedge \mathcal{C}_4 \wedge \mathcal{C}_5 \iff \max_\Omega \sum_{i \in I} \sum_{j \in I_j} \sum_{k \in K} x_{ijk} ( 1 - d(i,k) + \text{sev}(k) )
$$
</p>

One need to be careful then: all of these soft constraints don't have the same domain:
- Patients maximization constraint ranges from $0$ to $n$, with $n$ the number of patients,
- Severity constraint ranges from $0$ to $5\cdot n$
- Distance constraint ranges from $0$ to $\sqrt{(x_{Max}-x_{min})^2+(y_{Max}-y_{min})^2}\cdot n$

Given that all of these constraints share the same priority, one must define penalty factors to equilibrate the different constraints.

Here is the corresponding code:

<?prettify?>
    # Integer distance function
    idist = lambda xy1, xy2: int(((xy1[0]-xy2[0])**2 + (xy1[1]-xy2[1])**2)**0.5)

    # Gain factors (1/penalty factors)
    gain_max_patients = 140
    gain_severity = int(140/5)
    gain_distance = -1

    # Maximization objective
    soft_csts = []
    for i in range(n_hospitals):
      for j in range(n_beds_in_hospitals[i]):
        for k in range(n_patients):
          factor = \
            gain_max_patients \
            + gain_distance * idist(hospitals_loc[i], patients_loc[k]) \
            + gain_severity * patients_severity[k]
          soft_csts.append(factor * x[(i,j,k)])

    model.Maximize(sum(soft_csts))


### Solver

Now we can launch the solver. It will try to find the optimal solution within a specified time limit. If it can't manage to find the optimal solution, it will return the closest sub-optimal solution.

<?prettify?>
    solver = cp_model.CpSolver()
    solver.parameters.max_time_in_seconds = 60.0
    status = solver.Solve(model)

In our case, the solver returns an **optimal solution in 2.5 seconds** (2).

![Solution returned by the solver](/images/csp-map-2-inv.png =844x638)
*Fig. 2: Solution returned by the solver*

### Conclusion

To create this solution, all it takes is 1 hour of research and 30 minutes of programming. For a Deep Learning counterpart, one can predict a few days of data cleansing, at least a day to test different architectures and another day for training.

Moreover, a CP-SAT model is very robust if well modelized. Below are the results with different simulation parameters (3). Results are still coherent in many different cases, and with increased simulation parameters (3000 patients, 1000 beds), solution inference took a little less than 3 minutes.

![new results with other parameters](/images/csp-generations-inv.png =844x638)
*Fig. 3: Different simulation parameters*

Special thanks to [Laurent Perron](https://scholar.google.com/citations?user=umrglaIAAAAJ), and his Operations Research team at Google for their fantastic work, and for the time they take to answer technical questions on StackOverflow, GitHub and Google Groups.




### References 

[1] Jingchao Chen, *Solving Rubik's Cube Using SAT Solvers*, arXiv:1105.1436, 2011.

[2] Biere, A., Heule, M., and van Maaren, H. *Handbook of
satisfiability*, volume 185. IOS press, 2009a

[3] Knuth, D. E., *The art of computer programming*, Volume 4, Fascicle 6: Satisfiability. Addison-Wesley Professional, 2015

[4] Vipin Kumar, *Algorithms for constraint-satisfaction problems: a survey*, AI Magazine Volume 13, Issue 1, 1992.
