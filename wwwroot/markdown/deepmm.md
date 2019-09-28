##### Metamorphic programs mutate themselves upon execution

For software manufacturers, an important step before distributing an end product is to protect it against reverse engineering techniques that would open the door to piracy, or to technology stealing.

Any qualified person could use a specialized software known as a [disassembler](https://en.wikipedia.org/wiki/Disassembler) to convert back your binary executable into assembly code. It might be a pirate that would modify it in order to bypass your security protocols, or a competitor that want to understand the underlying logic or to reuse parts of your code.

![A disassembler in action](/images/deepmm-ida.png =632x535)
*Fig. 1: A disassembler in action - Image property of Hex-Rays*

### Code metamorphism

#### Methods

[Metamorphic programs](https://en.wikipedia.org/wiki/Metamorphic_code) can prevent or at least considerably slow down the process of reverse engineering, because *they will rewrite themselves at runtime*. Thus, a metamorphic program will change its structure, without changing its functionalities. 

There are two different ways to achieve this goal:
 - **High-level metamorphism**, where morphism is done with the same programming language used to code the software. It usually implies meta-programming frameworks (see [Boost.MPL](https://www.boost.org/doc/libs/1_71_0/libs/mpl/doc/index.html)) and compile time reflection libraries.
 - **Low-level metamorphism**, where the program disassembles itself and makes bitwise modifications, either on the fly in its machine code loaded in RAM, or in its binary file. Common used techniques are junk code cave replacement, instruction substitution, register swapping and code reordering *([Yeong T. Ling et al., 2017](https://pdfs.semanticscholar.org/948a/0d1200ff4416e15550cce92e7f0406b6d002.pdf))*.

![Register swapping is a common technique of low-level metamorphism](/images/deepmm-regswap.jfif =620x144)
*Fig. 2: Register swapping is a common technique of low-level metamorphism - From Yeong T. Ling et al., 2017*

#### State of the art

**High-level metamorphism** is a very advanced topic and a limited amount of solutions have been developed, with very specific use cases. A notable example would be the C library for discrete Fourier transforms [FFTW](https://fftw.org), whose code is generated automatically using OCaml scripting language *([M. Frigo, 1999](http://www.fftw.org/pldi99.pdf))*.

**Low-level metamorphism** is used much more often and has found many applications. Unfortunatly, it has especially proved its usefulness for malware obfuscating *([M. Stamp et al., 2010](https://www.researchgate.net/publication/228984341_A_highly_metamorphic_virus_generator))*. Moreover, those solutions are bound to recognize known instruction patterns and swap them by equivalents. **Hence, a well trained heuristic agent could predict those changes.**

Ideally, a perfect metamorphic engine could receive any piece of code of variable length, and output a roughly equivalent but structurally different code.

### Deep MetaMorph

Deep MetaMorph is proof of concept to provide a *low-level metamorphism solution* using deep learning, without the aforementioned drawback.
 
#### Concept

**Machine code is executed sequentially.** Each instruction is the combination of an *opcode* and several *arguments*. For instance, within the x86 instruction set, the instruction `MOV EAX, EBX` would be represented as `66 89 D8` in hexadecimal. The opcode for the instruction "move a 32 bits register into another 32 bits register" is `66 89`, and the arguments would be the two registers, represented as `D8`.

The instruction modifies the **state of the program**. The latter is made of the processor registers, the stack, the heap, and other virtual memory sections that are mapped by our program.

If we make an observation of the whole state of the program before and after running an unknown instruction, just by studying the differences between those two observations, we should be able to guess what instruction it was, or at least to find a instruction sequence that fits. Because of that, the machine code execution process is a **Markov chain**.

Thus, if we train a model to infer a sequence of instructions that would transform one program state in another, we could use it to mutate existing code.

Let $\mathcal{E}$ denote the space of the program states. Hence, an instruction can be denoted as $i: \mathcal{E} \rightarrow \mathcal{E}$.

Let $n \in \\\ \mathbb{N}^\star$, and $(i_k)_{k\in[[0,n]]}$, such as $\forall k, i_k : \mathcal{E} \rightarrow \mathcal{E}$, a finite sequence of instructions.

Given $E_1,E_2 \in\mathcal{E}$ two distinct program states, for our metamorphic engine, we must find $(j_k)_{k\in[[0,m]]}, m \in \\\ \mathbb{N}^\star$, such as  :

$$(j_0 \circ \ldots \circ j_m)(E_1) = (i_0 \circ \ldots \circ i_n)(E_1) = E_2$$

It means that for each sequence of instructions we want to replace, we must find another sequence which transform the same initial state into the same final state. 

In practice, we also want $m=n$, because it is technically very complicated to replace a sequence of instructions with another sequence of different length. However, there exist an identity operation in $\mathcal{E}$, known as the `NOP` instruction. 

Hence, in the case where $m < n$, we can always fill the sequence with `NOP` instructions.

#### From theory to practice

Capturing the whole state of the program between two instructions is unrealistic. As I am writing those lines, my google chrome instance is using 800MB of RAM. Bitwise speaking, it would be $6.4\cdot 10^9$ parameters to capture and process at each iteration of the metamorphic engine.

To reduce the amount of parameters, we will be working on a restricted space $\mathcal{E}_r \\\ \subset\mathcal{E}$, composed of the following registers: `EAX`, `EBX`, `ECX`, `EDX`, `EDI`, `ESI` and `EFLAGS`, and of the first 16 32-bits variables on the stack: by its [LIFO structure](https://en.wikipedia.org/wiki/Stack_(abstract_data_type)), many of the interactions are made on top of the stack. The heap and other memory structures that require addressing won't be observed. 

Hence, in our case, $\mathcal{E}_r$ is equinumerous to $\lbrace 0, 1 \rbrace ^{736}$, a vector containing all the bits of the aforementioned observations.

By using this simplification, we only need to observe $i_r: \\\ \mathcal{E}_r \rightarrow \mathcal{E}_r$, so we must skip every instruction related to:

- Data segments
- Pointers
- GPU instructions
- Function calls/returns or jumps

There are still plently of [x86 instructions](https://www.intel.com/content/dam/www/public/us/en/documents/manuals/64-ia-32-architectures-software-developer-instruction-set-reference-manual-325383.pdf) to study given those restrictions.

#### Data aggregation

To build our dataset, we need to feed a debugger with any compiled program, to run it step by step, and to capture $\mathcal{E}_r$ before and after every fitting instruction. This process has been automated using gdb and its python bindings. 

To avoid being stuck in event pumps or in low level loops, the stepping is made in *step over* mode. Hence, we need to set breakpoints at the entry of many functions. Thus, we preferably need to scrap programs compiled with debug symbols.

This way, if we are stuck in a repeating pattern of functions, we can simply continue the execution until the next breakpoint.

![The scrapping process with gdb](/images/deepmm-extract.jpg =632x457)
*Fig. 3: Data aggregation process*

This technique made me aggregate a dataset of roughly 200,000 instruction sequences $(i_n)$, and their associated program states.

After that, the data needs to be analysed for outliers and consistency, then tidied in order to be fed to a neural network. This process was done using the R programming language, as well as some data visualization.

In the following figure *(fig. 4)*, each row $i$ corresponds to a different 32-bit value, either a register or a variable on the stack. A column $j$ represent the index of the bits (little endian). The color of a pixel $s_{ij}$ represents **how often the $j$-th bit of the $i$-th variable has changed before and after an instruction**.

More rigorously, with $m$ the number of instruction samples, if we denote $A_k^{before},A_k^{after}\in\\ \mathcal{E}\_{r}$ the representations of the program states before and after the instruction  $k\in [[1,m]]$, then the matrix $S_{ij}$ depicted below can be computed as:

$$ S = \sum_{k=1}^m \lvert A_k^{after} - A_k^{before} \rvert$$

![The scrapping process with gdb](/images/deepmm-heatmap.png =750x458)
*Fig. 4: Data visualisation*

The stack usage is indeed homogeneous: each push/pop instruction will move the entire stack up and down. For the registers, their usage is a bit more specific. First of all, `EDI` and `ESI` are addressing registers. Hence, by our scrapping rules they are barely not used. Next, the 2nd, 4th and 6th bits of `EFLAGS` are reserved in the x86 architecture, and are not used at all. Same goes for all bits whose index is greater than 8. Similarly, there are a lot of comparisons between this graph and the x86 architecture model.
 
#### Implementation

We have now an output dataset $Y_{m}^{(n_y)\<T\>}$ composed of $m$ sequences of length $T$ made of x86 instructions taken from a dictionary of length $n_y$.

To this is mapped an input dataset $X_{m}^{(n_x)\<T\>}$ of the same $m$ sequences of length $T$ composed of the program states ($n_x$ dimensions) before each corresponding instruction in $Y_{m}^{(n_y)\<T\>}$.

The goal was to build a simple sequence to sequence model in Keras able to infer the instructions $y^{\<T\>}$ able to transition from the program states $x^{\<T\>}$.

The whole model is made of an inference encoder and decoder with LSTM cells, and was build using Keras functional API.

![The Keras model](/images/deepmm-model.png =319x591)
*Fig. 5: The Keras model*

The source code for the data aggregation, dataset consolidation and model building and training can be found [here, on my github](https://github.com/antoinechampion/DeepMetaMorph).
The repository also includes the weigths of the model after a first training.

### Results

The dataset was split into 90% of training data and 10% of validation data.

After 60 epochs of training (minibatches), the model capped to an accuracy of 0.89. There was no sign of overfitting whatsoever : for now, the accuracy and the robustness against new cases could be increased using a larger dataset.

Yet, it isn't ready for a production environment. One can imagine an heuristic agent that would start at a program launch and would perform the following steps: 
- Take random chunks of instructions and capture program states,
- Feed the states to the model and predict a new sequence of instructions,
- Replace the old sequence of instructions by the new one in memory or in the binary file.

Such a machine learning approach is also quite resource intensive for a real time use. The heuristic engine could include precomputed mutations of various parts of the program that would be mixed together to produce an unique result.
Those mutations could be stored in a remote facility such as a spark/hadoop cluster to prevent their exposition to the end user.

### Summary

This whole study is a very promising approach with good results but still state-of-the-art. I couldn't find any other publication on the matter of generating metamorphic code using machine learning techniques. Further research need to be conducted on the creation of larger datasets and the experimentation of different models.

Don't hesitate to [contact me](mailto:contact@antoinechampion.com) if you need some valuable insights on the topic.
